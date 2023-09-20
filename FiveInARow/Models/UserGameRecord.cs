using System.ComponentModel.DataAnnotations;

namespace FiveInARow.Models
{
    public class UserGameRecord
    {
        [Required]
        public int UserId { get; }
        [Required]
        public int GameRecordId { get; }
        [Required]
        public User User { get; }
        [Required]
        public GameRecord GameRecord { get; }

    }
    
}