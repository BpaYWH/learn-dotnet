namespace FiveInARow.Models
{
    public class GameState
    {
        public Guid GameId { get; set; }
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
        public List<GameMove> Moves { get; set; }
        public string CurrentPlayerId { get; set; }
        public bool IsFinished { get; set; }
        public string WinnerId { get; set; }
        public int[,] Board { get; set; } = new int[15, 15];
    }
}