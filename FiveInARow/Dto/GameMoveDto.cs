namespace FiveInARow.Dto
{
    public class GameMoveDto
    {
        public int Id { get; }
        public string GameId { get; }
        public string PlayerId { get; } 
        public int Row { get; }
        public int Column { get; }
        public DateTime TimeStamp { get; }
    }
}