using FiveInARow.Context;
using FiveInARow.Models;

namespace FiveInARow.Services.FiveInARow
{
    public class GameRecordService : IGameRecordService
    {
        private readonly FiveInARowDbContext _context;

        public GameRecordService(FiveInARowDbContext context)
        {
            _context = context;
        }

        public bool CreateGameRecord(GameRecord gameRecord)
        {
            _context.GameRecords.Add(gameRecord);
            return Save();
        }

        public bool GameRecordExists(int id)
        {
            return _context.GameRecords.Any(gr => gr.Id == id);
        }

        public GameRecord GetGameRecord(int id)
        {
            return _context.GameRecords.FirstOrDefault(gr => gr.Id == id);
        }

        public ICollection<GameRecord> GetGameRecords()
        {
            return _context.GameRecords.OrderBy(gr => gr.Id).ToList();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}