using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Practice.Services.msmovies.Models
{
    public class Movie
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("genre")]
        public string Genre { get; set; } = string.Empty;

        [BsonElement("year")]
        public int Year { get; set; } = 2024;

        [BsonElement("releaseDate")]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        [BsonElement("rating")]
        public double Rating { get; set; } = 0;

        [BsonElement("stock")]
        public int Stock { get; set; } = 100;
    }
}