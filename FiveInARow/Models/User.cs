using System.ComponentModel.DataAnnotations;

namespace FiveInARow.Models
{
    public class User
    {
        [Key]
        public int Id { get; }
        
        [Required]
        public string Name { get; }

        [Required]
        public string Password { get; }

        [Required]
        [EmailAddress]
        public string Email { get; }

        public DateTime CreatedAt { get; set; }

        public ICollection<UserGameRecord> UserGameRecords { get; }

    }
}
