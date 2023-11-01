namespace FiveInARow.Dto
{
    public class GameMoveDto
    {
        // public int Id { get; }
        public string GameId { get; set; }
        public string PlayerId { get; set; } 
        public int Row { get; set; }
        public int Col { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}