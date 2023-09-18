namespace FiveInARow.Contracts.FiveInARow
{
    public record CreateGameRecordRequest
    (
        int Player1Id,
        int Player2Id,
        DateTime StartTime,
        DateTime EndTime,
        int WinnerId
    );
}