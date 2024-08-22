using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Movies.Models
{
    public class Movie
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("genre")]
        public string Genre { get; set; } = string.Empty;

        [BsonElement("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [BsonElement("rating")]
        public double Rating { get; set; }
    }
}