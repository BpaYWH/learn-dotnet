using FiveInARow.Models;

namespace FiveInARow.Contracts.FiveInARow
{
    public record UserResponse
    (
        int Id,
        string Name,
        string Email,
        DateTime CreatedAt,
        ICollection<GameRecord> GameRecords
    );
}
