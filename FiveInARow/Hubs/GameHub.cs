using System.Collections.Concurrent;
using System.Numerics;
using AutoMapper;
using FiveInARow.Models;
using FiveInARow.Repositories;
using FiveInARow.Services.FiveInARow;
using FiveInARow.Dto;
using Microsoft.AspNetCore.SignalR;

namespace FiveInARow.Hubs
{
    public sealed class GameHub : Hub
    {
        // IGameRepository _gameRepository;
        // ConcurrentDictionary<string, User> _connectedUsers;
        private readonly IMapper _mapper;
        private static ConcurrentDictionary<string, HashSet<string>> gameRooms = new ();
        private static ConcurrentDictionary<string, GameState> gameStates = new ();

        private bool RoomExists(string roomId)
        {
            return gameRooms.ContainsKey(roomId);
        }
        private void AddRoom(string roomId)
        {
            gameRooms.AddOrUpdate(roomId, new HashSet<string>(), (key, value) => value);
        }
        private void JoinRoom(string roomId, string playerId) {
            gameRooms[roomId].Add(playerId);
        }
        
        private void LeaveRoom(string roomId, string playerId) 
        {
            gameRooms.TryGetValue(roomId, out var players);
            if (players == null) {
                return;
            }

            players.Remove(playerId);
        }
        private bool IsRoomFull(string roomId)
        {
            gameRooms.TryGetValue(roomId, out var players);
            
            if (players == null) return false;

            return players.Count > 1;
        }
        
        private GameState? InitGameState(string roomId)
        {
            gameRooms.TryGetValue(roomId, out var players);
            if (players == null || players.Count < 2) return null;

            Guid gameId = Guid.NewGuid();
            GameState gameState = new ()
            {
                GameId = gameId,
                Player1Id = players.First(),
                Player2Id = players.Last(),
                Moves = new List<GameMove>(),
                CurrentPlayerId = players.First(),
                IsFinished = false,
            };

            gameStates.AddOrUpdate(roomId, gameState, (key, value) => value);

            return gameState;
        }

        private bool ValidateMove(string roomId, GameMove move)
        {
            gameStates.TryGetValue(roomId, out var gameState);
            if (gameState == null) return false;

            var board = gameState.Board;
            var currentPlayerId = gameState.CurrentPlayerId;

            if (move.Row < 0 || move.Row >= board.GetLength(0) || move.Col < 0 || move.Col >= board.GetLength(1)) return false;
            if (board[move.Row, move.Col] != 0 || currentPlayerId != move.PlayerId) return false;

            return true;            
        }

        private static bool IsGameSet(int[,] board, int side)
        {
            // check horizontal
            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1)-4; j++) {
                    if (board[i,j] == side && board[i,j+1] == side && board[i,j+2] == side && board[i,j+3] == side && board[i,j+4] == side)
                        return true;
                }
            }

            // check vertical
            for (int i = 0; i < board.GetLength(1); i++) {
                for (int j = 0; j < board.GetLength(0)-4; j++) {
                    if (board[j,i] == side && board[j+1,i] == side && board[j+2,i] == side && board[j+3,i] == side && board[j+4,i] == side)
                        return true;
                }
            }

            // check forward diagonal /
            for (int i = board.GetLength(0) - 1; i > 3; i--) {
                for (int j = 0; j < board.GetLength(1) - 4; j++) {
                    if (board[i,j] == side && board[i-1,j+1] == side && board[i-2,j+2] == side && board[i-3,j+3] == side && board[i-4,j+4] == side)
                        return true;
                }
            }

            // check backward diagonal \
            for (int i = 0; i < board.GetLength(0) - 4; i++) {
                for (int j = 0; j < board.GetLength(1) - 4; j++) {
                    if (board[i,j] == side && board[i+1,j+1] == side && board[i+2,j+2] == side && board[i+3,j+3] == side && board[i+4,j+4] == side)
                        return true;
                }
            }

            return false;
        }

        private GameState? GetGameState(string roomId)
        {
            gameStates.TryGetValue(roomId, out var gameState);
            return gameState;
        }
        private void UpdateGameState(string roomId, GameMove move) 
        {
            gameStates.TryGetValue(roomId, out var gameState);
            if (gameState == null) return;

            // update game state
            var currentPlayerId = gameState.CurrentPlayerId;

            gameState.Board[move.Row, move.Col] = currentPlayerId == gameState.Player1Id ? 1 : 2;
            gameState.CurrentPlayerId = currentPlayerId == gameState.Player1Id ? gameState.Player2Id : gameState.Player1Id;
            gameState.Moves.Add(move);

            // check win
            gameState.IsFinished = IsGameSet(gameState.Board, currentPlayerId == gameState.Player1Id ? 1 : 2);

            return;
        }
        private void EndGame(string roomId) 
        {
            gameStates.TryGetValue(roomId, out var gameState);
            if (gameState != null) {
                gameState.IsFinished = true;
                // store the game moves
            }

            gameStates.TryRemove(roomId, out var _);
        }

        private async void ClearRoom(string roomId)
        {
            gameRooms.TryGetValue(roomId, out var players);
            if (players == null) return;

            foreach (string playerId in players) {
                await Clients.Groups(roomId).SendAsync("RoomLeft", playerId);
                await Groups.RemoveFromGroupAsync(playerId, roomId);
            }

            gameRooms.TryRemove(roomId, out var _);
        }

        private bool IsPlayerInRoom(string roomId, string playerId)
        {
            gameRooms.TryGetValue(roomId, out var players);
            return players != null && players.Contains(playerId);
        }

        private bool IsInGame(string roomId) 
        {
            gameStates.TryGetValue(roomId, out var gameState);
            return gameState != null && !gameState.IsFinished;
        }

        public GameHub(IConfiguration configuration, IMapper mapper)
        {
            var connectionString = configuration.GetConnectionString("GomokuDb") ?? "";
            // _gameRepository = new GameRepository(connectionString);
            // _connectedUsers = new ();
            _mapper = mapper;
        }

        public async Task CreateGame()
        {
            string roomId = Context.ConnectionId;

            if (!RoomExists(roomId))
            {
                // If the provided roomId doesn't exist, generate a new one
                AddRoom(roomId);
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await Clients.Caller.SendAsync("RoomCreated", roomId);
            } 
            else 
            {
                await Clients.Caller.SendAsync("RoomNotCreated", roomId);
                Context.Abort();
            }
        }

        public async Task JoinGame(string roomId)
        {
            if (RoomExists(roomId) && !IsRoomFull(roomId))
            {
                JoinRoom(roomId, Context.ConnectionId);
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await Clients.Caller.SendAsync("RoomJoined", roomId);
                await Clients.Group(roomId).SendAsync("PlayerConnected", Context.ConnectionId);
            }
            else
            {
                await Clients.Caller.SendAsync("RoomNotFound", roomId);
                Context.Abort();
            }
        }

        public async Task StartGame(string roomId)
        {
            // initialize game state
            GameState? gameState =  InitGameState(roomId);
            if (gameState == null)
            {
                await Clients.Group(roomId).SendAsync("GameNotStarted", "Not enough players");
            }
            else
            {
                await Clients.Group(roomId).SendAsync("GameStarted", gameState);
            }
        }

        public async Task LeaveGame(string roomId) 
        {
            if (RoomExists(roomId))
            {
                if (IsPlayerInRoom(roomId, Context.ConnectionId) && IsInGame(roomId))
                {
                    EndGame(roomId);
                    ClearRoom(roomId);
                }
                else
                {
                    LeaveRoom(roomId, Context.ConnectionId);
                    await Clients.Group(roomId).SendAsync("RoomLeft", Context.ConnectionId);
                }
            }

            Context.Abort();
        }

        public async Task Move(string roomId, GameMoveDto gameMove)
        {
            // check valid player
            if (!IsPlayerInRoom(roomId, Context.ConnectionId) || !IsInGame(roomId))
            {
                await Clients.Caller.SendAsync("InvalidGame", "Invalid game");
                return;
            }

            // validate move
            GameMove gameMoveMap = _mapper.Map<GameMove>(gameMove);
            if (!ValidateMove(roomId, gameMoveMap))
            {
                await Clients.Caller.SendAsync("InvalidMove", "Invalid move");
                return;
            }

            // update game state
            UpdateGameState(roomId, gameMoveMap);
            var gameState = GetGameState(roomId);

            // return updated game state (updated currentPlayer, last move, isFinished are enough)
            await Clients.Group(roomId).SendAsync("GameUpdated", gameState);

            // handle game set
            if (gameState != null && gameState.IsFinished)
            {
                EndGame(roomId);
                ClearRoom(roomId);
            }
        }

        // Announcement
    }
}