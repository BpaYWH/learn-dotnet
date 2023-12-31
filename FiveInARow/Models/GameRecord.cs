using System.ComponentModel.DataAnnotations;

namespace FiveInARow.Models
{
    public class GameRecord
    {
        [Key]
        public int Id { get; }

        [Required]
        public int Player1Id { get; }

        [Required]
        public int Player2Id { get; }

        public int WinnerId { get; }

        public DateTime StartedAt { get; }

        public DateTime EndedAt { get; }

        public ICollection<UserGameRecord> UserGameRecords { get; }
    }
}
