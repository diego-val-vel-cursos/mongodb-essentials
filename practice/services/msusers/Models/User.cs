using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Movies.Models;

namespace Users.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;


        [BsonElement("lastname")]
        public string LastName { get; set; } = string.Empty;


        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        [BsonElement("datecreated")]
        public DateTime DateCreated { get; set; }

        [BsonElement("movies")]
        public Movie[] Movies { get; set; } = new Movie[0];
    }
}
