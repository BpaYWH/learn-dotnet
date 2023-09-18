namespace FiveInARow.Models
{
    public class UserGameRecord
    {
        public int UserId { get; set; }
        public int GameRecordId { get; set; }
        public User User { get; set;} = new ();
        public GameRecord GameRecord { get; set;} = new ();
    }
    
}