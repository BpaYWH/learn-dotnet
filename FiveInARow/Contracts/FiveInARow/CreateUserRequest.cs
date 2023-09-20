
namespace FiveInARow.Contracts.FiveInARow
{
    public record CreateUserRequest
    (
        int Id,
        string Name,
        string Email,
        string Password,
        DateTime CreatedAt,
        List<int> GameRecords
    );
}

