namespace FiveInARow.Dto
{
    public class GameRecordDto
    {
        public int Id { get; }
        public int Player1Id { get; }
        public int Player2Id { get; }
        public int WinnerId { get; }
        public DateTime StartedAt { get; }
        public DateTime EndedAt { get; }
    }
}