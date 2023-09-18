namespace FiveInARow.Contracts.FiveInARow
{
    public record GameRecordRequest
    (
        int Player1Id,
        int Player2Id,
        DateTime StartTime,
        DateTime EndTime,
        int WinnerId
    );
}