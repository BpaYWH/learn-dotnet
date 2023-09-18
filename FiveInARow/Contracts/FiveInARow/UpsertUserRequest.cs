
namespace FiveInARow.Contracts.FiveInARow
{
    public record UpsertUserResponse
    (
        int Id,
        string Name,
        string Email,
        ICollection<int> GameRecords
    );
}

