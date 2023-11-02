using System.Numerics;
using AutoMapper;
using FiveInARow.Models;
using FiveInARow.Repositories;
using FiveInARow.Services.FiveInARow;
using FiveInARow.Dto;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace FiveInARow.Hubs
{
    public sealed class GameHub : Hub
    {
        IGameRepository _gameRepository;
        // ConcurrentDictionary<string, User> _connectedUsers;
        private readonly IMapper _mapper;
        

        public GameHub(IConfiguration configuration, IMapper mapper)
        {
            // var connectionString = configuration.GetConnectionString("GomokuDb") ?? "";
            _gameRepository = new GameRepository();
            // _connectedUsers = new ();
            _mapper = mapper;
        }

        public async Task CreateGame()
        {
            string roomId = Context.ConnectionId;

            if (!_gameRepository.RoomExists(roomId))
            {
                // If the provided roomId doesn't exist, generate a new one
                _gameRepository.AddRoom(roomId);
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await Clients.Caller.SendAsync("RoomCreated", roomId, roomId);
            } 
            else 
            {
                await Clients.Caller.SendAsync("RoomNotCreated");
                Context.Abort();
            }
        }

        public async Task JoinGame(string roomId)
        {
            if (_gameRepository.RoomExists(roomId) && !_gameRepository.IsRoomFull(roomId))
            {
                _gameRepository.JoinRoom(roomId, Context.ConnectionId);
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await Clients.Caller.SendAsync("RoomJoined", roomId, Context.ConnectionId);
                await Clients.Group(roomId).SendAsync("PlayerConnected", Context.ConnectionId, _gameRepository.IsRoomFull(roomId));
            }
            else
            {
                await Clients.Caller.SendAsync("RoomNotFound");
                Context.Abort();
            }
        }

        public async Task StartGame(string roomId)
        {
            // initialize game state
            GameState? gameState =  _gameRepository.InitGameState(roomId);
            if (gameState == null)
            {
                await Clients.Group(roomId).SendAsync("GameNotStarted", "Not enough players");
            }
            else
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(gameState);
                await Clients.Group(roomId).SendAsync("GameStarted", jsonString);
            }
        }

        public async Task LeaveGame(string roomId) 
        {
            if (_gameRepository.RoomExists(roomId))
            {
                if (_gameRepository.IsPlayerInRoom(roomId, Context.ConnectionId) && _gameRepository.IsInGame(roomId))
                {
                    _gameRepository.EndGame(roomId);
                    _gameRepository.ClearRoom(roomId);
                }
                else
                {
                    _gameRepository.LeaveRoom(roomId, Context.ConnectionId);
                    await Clients.Group(roomId).SendAsync("RoomLeft", Context.ConnectionId);
                }
            }

            Context.Abort();
        }

        public async Task Move(string roomId, GameMoveDto gameMove)
        {
            // check valid player
            if (!_gameRepository.IsPlayerInRoom(roomId, Context.ConnectionId) || !_gameRepository.IsInGame(roomId))
            {
                await Clients.Caller.SendAsync("InvalidGame", "Invalid game");
                return;
            }

            // validate move
            GameMove gameMoveMap = _mapper.Map<GameMove>(gameMove);
            if (!_gameRepository.ValidateMove(roomId, gameMoveMap))
            {
                await Clients.Caller.SendAsync("InvalidMove", "Invalid move");
                return;
            }

            // update game state
            _gameRepository.UpdateGameState(roomId, gameMoveMap);
            var gameState = _gameRepository.GetGameState(roomId);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(gameState);

            // return updated game state (updated currentPlayer, last move, isFinished are enough)
            await Clients.Group(roomId).SendAsync("GameUpdated", jsonString);

            // handle game set
            if (gameState != null && gameState.IsFinished)
            {
                await Clients.Group(roomId).SendAsync("GameSet");
                _gameRepository.EndGame(roomId);
                _gameRepository.ClearRoom(roomId);
            }
        }

        // Announcement
    }
}