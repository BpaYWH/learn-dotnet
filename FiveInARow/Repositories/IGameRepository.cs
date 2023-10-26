using FiveInARow.Models;

namespace FiveInARow.Repositories
{
    public interface IGameRepository
    {
        bool CreateGameMove(GameMove gameMove);

        GameMove GetGameMove();

        bool UpsertGameMoveCache(Move move);

        bool Save();
    }
}