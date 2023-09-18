using System.ComponentModel.DataAnnotations;

namespace FiveInARow.Models
{
    public class GameRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public User Player1 { get; set; } = new ();

        [Required]
        public User Player2 { get; set; } = new ();

        public User Winner { get; set; } = new ();

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ICollection<UserGameRecord>? UserGameRecords { get; set; }
    }
}
