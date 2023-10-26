using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FiveInARow.Models
{
    public class GameMove
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; }

        [BsonElement("player1Id")]
        public int Player1Id { get; }

        [BsonElement("player2Id")]
        public int Player2Id { get; }

        [BsonElement("moves")]
        public List<Move> Moves { get; set; }
    }

    public class Move
    {
        public Tuple<int, int, DateTime> MoveData { get; set; }

    }
}