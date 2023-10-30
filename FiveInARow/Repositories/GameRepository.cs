using FiveInARow.Models;

namespace FiveInARow.Repositories
{
    public class GameRepository : IGameRepository
    {
        // private readonly string _connectionString;
        // private GameMove _gameMove;

        // public GameRepository(string connectionString)
        // {
            // _connectionString = connectionString;
            // _gameMove = new GameMove();
        // }

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
    }
}