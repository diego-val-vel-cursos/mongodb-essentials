using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBEssentials.Models
{
    public class MarketingMember
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("Age")]
        public string Age { get; set; } = string.Empty;
    }
}
