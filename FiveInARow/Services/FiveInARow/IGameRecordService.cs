using FiveInARow.Models;

namespace FiveInARow.Services.FiveInARow
{
    public interface IGameRecordService
    {
        bool CreateGameRecord(GameRecord gameRecord);
        GameRecord GetGameRecord(int id);

        ICollection<GameRecord> GetGameRecords();

        bool GameRecordExists(int id);

        bool Save();
    }
}