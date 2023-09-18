
namespace FiveInARow.Contracts.FiveInARow
{
    public record UserResponse
    (
        int Id,
        string Name,
        string Email,
        ICollection<int> GameRecords
    );
}

