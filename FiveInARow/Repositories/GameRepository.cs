using System.Collections.Concurrent;
using FiveInARow.Models;

namespace FiveInARow.Repositories
{
    public class GameRepository : IGameRepository
    {
        private static ConcurrentDictionary<string, HashSet<string>> gameRooms = new ();
        private static ConcurrentDictionary<string, GameState> gameStates = new ();
        // private string _connectionString;
        // private GameMove _gameMove;

        public GameRepository()
        {
            // _connectionString = connectionString;
            // _gameMove = new GameMove();
        }

        public bool CreateGameMove(GameMove gameMove)
        {
            throw new NotImplementedException();
        }

        public GameMove GetGameMove()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool UpsertGameMoveCache()
        {
            // var moveLength = _gameMove.Moves.Count;
            // _gameMove.Moves.Add(move);
            // return _gameMove.Moves.Count > moveLength;
            throw new NotImplementedException();
        }

        public bool RoomExists(string roomId)
        {
            return gameRooms.ContainsKey(roomId);
        }
        public void AddRoom(string roomId)
        {
            gameRooms.AddOrUpdate(roomId, new HashSet<string> {roomId}, (key, value) => value);
        }
        public void JoinRoom(string roomId, string playerId) 
        {
            gameRooms[roomId].Add(playerId);
        }
        
        public void LeaveRoom(string roomId, string playerId) 
        {
            gameRooms.TryGetValue(roomId, out var players);
            if (players == null) {
                return;
            }

            players.Remove(playerId);
        }
        public bool IsRoomFull(string roomId)
        {
            gameRooms.TryGetValue(roomId, out var players);
            
            if (players == null) return false;

            return players.Count > 1;
        }
        
        public GameState? InitGameState(string roomId)
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

        public bool ValidateMove(string roomId, GameMove move)
        {
            gameStates.TryGetValue(roomId, out var gameState);
            if (gameState == null) 
            {
                return false;
            }

            var board = gameState.Board;
            var currentPlayerId = gameState.CurrentPlayerId;

            if (move.Row < 0 || move.Row >= board.GetLength(0) || move.Col < 0 || move.Col >= board.GetLength(1)) 
            {
                return false;
            }
            if (board[move.Row, move.Col] != 0 || currentPlayerId != move.PlayerId) 
            {
                return false;
            }

            return true;            
        }

        public bool IsGameSet(int[,] board, int side)
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

        public GameState? GetGameState(string roomId)
        {
            gameStates.TryGetValue(roomId, out var gameState);
            return gameState;
        }
        public void UpdateGameState(string roomId, GameMove move) 
        {
            gameStates.TryGetValue(roomId, out var gameState);
            if (gameState == null) return;

            // update game state
            var currentPlayerId = gameState.CurrentPlayerId;

            gameState.Board[move.Row, move.Col] = currentPlayerId == gameState.Player1Id ? 1 : 2;
            gameState.Moves.Add(move);

            // check win
            gameState.IsFinished = IsGameSet(gameState.Board, currentPlayerId == gameState.Player1Id ? 1 : 2);
            if (gameState.IsFinished) {
                gameState.WinnerId = currentPlayerId;
                return;
            }

            // update current player
            gameState.CurrentPlayerId = currentPlayerId == gameState.Player1Id ? gameState.Player2Id : gameState.Player1Id;

            return;
        }
        public void EndGame(string roomId) 
        {
            gameStates.TryGetValue(roomId, out var gameState);
            if (gameState != null) {
                gameState.IsFinished = true;
                // store the game moves
            }

            gameStates.TryRemove(roomId, out var _);
        }

        public void ClearRoom(string roomId)
        {
            gameRooms.TryGetValue(roomId, out var players);
            if (players == null) return;

            foreach (string playerId in players) {
                gameRooms[roomId].Remove(playerId);
            }

            gameRooms.TryRemove(roomId, out var _);
        }

        public bool IsPlayerInRoom(string roomId, string playerId)
        {
            gameRooms.TryGetValue(roomId, out var players);
            return players != null && players.Contains(playerId);
        }

        public bool IsInGame(string roomId) 
        {
            gameStates.TryGetValue(roomId, out var gameState);
            return gameState != null && !gameState.IsFinished;
        }

        public bool IsHost(string playerId, string roomId) {
            return playerId == roomId;
        }
    }
}