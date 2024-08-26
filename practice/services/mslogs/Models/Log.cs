using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Practice.Services.mslogs.Models
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("action")]
        public string Action { get; set; } = string.Empty;

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("details")]
        public string Details { get; set; } = string.Empty;

        [BsonElement("microservice")]
        public string Microservice { get; set; } = string.Empty;

        [BsonElement("endpoint")]
        public string Endpoint { get; set; } = string.Empty;
    }
}
