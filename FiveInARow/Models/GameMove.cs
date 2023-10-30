using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FiveInARow.Models
{
    public class GameMove
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GameId { get; }

        [BsonElement("playerId")]
        public string PlayerId { get; }

        [BsonElement("row")]
        public int Row { get; set; }

        [BsonElement("col")]
        public int Col { get; set; }
        
        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}