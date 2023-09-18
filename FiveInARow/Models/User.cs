using System.ComponentModel.DataAnnotations;

namespace FiveInARow.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set;} = String.Empty;

        public ICollection<UserGameRecord>? UserGameRecords { get; set; }
    }
}
