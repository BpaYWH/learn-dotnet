using FiveInARow.Models;

namespace FiveInARow.Repositories
{
    public interface IGameRepository
    {
        bool CreateGameMove(GameMove gameMove);

        GameMove GetGameMove();

        bool UpsertGameMoveCache();

        bool Save();

        bool RoomExists(string roomId);

        void AddRoom(string roomId);

        void JoinRoom(string roomId, string playerId);

        void LeaveRoom(string roomId, string playerId);

        bool IsRoomFull(string roomId);

        GameState? InitGameState(string roomId);

        bool ValidateMove(string roomId, GameMove move);

        bool IsGameSet(int[,] board, int side);

        GameState? GetGameState(string roomId);

        void UpdateGameState(string roomId, GameMove move);

        void EndGame(string roomId);

        void ClearRoom(string roomId);

        bool IsPlayerInRoom(string roomId, string playerId);

        bool IsInGame(string roomId);

        bool IsHost(string playerId, string roomId);
    }
}