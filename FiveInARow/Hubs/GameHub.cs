using System.Collections.Concurrent;
using FiveInARow.Models;
using FiveInARow.Repositories;
using FiveInARow.Services.FiveInARow;
using Microsoft.AspNetCore.SignalR;

namespace FiveInARow.Hubs
{
    public sealed class GameHub : Hub
    {
        IGameRepository _gameRepository;
        ConcurrentDictionary<string, User> _connectedUsers;


        public GameHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("GomokuDb") ?? "";
            // _gameRepository = new GameRepository(connectionString);
            _connectedUsers = new ();
        }

        public async Task CreateRoom(User user)
        {
        }

        public override async Task OnConnectedAsync()
        {
            // await Clients.All.SendAsync(Context.User.Identity.Name, "joined");
            await Clients.All.SendAsync("PlayerConnected", "Hello World");
            await base.OnConnectedAsync();
        }

        public async Task NewMessage(string message){
            await Clients.All.SendAsync("messageReceived", message);
        }

        public async Task JoinRoom(User user, Guid roomId)
        {
        }

        public async Task LeaveRoom(User user, Guid roomId)
        {
        }

        public async Task CloseRoom(User user, Guid roomId)
        {
        }

        public async Task StartGame(User user, Guid roomId)
        {
        }

        public async Task LeaveGame(User user, Guid roomId)
        {
        }

        public async Task Move(User user, Move move, Guid roomId)
        {

        }

        public async Task EndGame(Guid roomId)
        {
            
        }
    }
}