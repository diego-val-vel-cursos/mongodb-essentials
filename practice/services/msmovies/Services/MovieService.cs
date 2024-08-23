using MongoDB.Driver;
using MongoDB.Bson;
using Practice.Services.msmovies.Models;
using Microsoft.Extensions.Options;

namespace Practice.Services.msmovies.Services
{
    public class MovieService
    {
        private readonly IMongoCollection<Movie> _movies;
        private readonly ILogger<MovieService> _logger;

        public MovieService(IOptions<MongoDBSettings> settings, ILogger<MovieService> logger)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            
            _movies = database.GetCollection<Movie>(settings.Value.MoviesCollectionName);
            _logger = logger;

            // Create a simple index on the "Title" field
            var indexKeysDefinition1 = Builders<Movie>.IndexKeys.Ascending(movie => movie.Title);
            _movies.Indexes.CreateOne(new CreateIndexModel<Movie>(indexKeysDefinition1));

            // Create a compound index on the "Genre" and "Year" fields
            var indexKeysDefinition = Builders<Movie>.IndexKeys
                .Ascending(movie => movie.Genre)
                .Ascending(movie => movie.Year);
            _movies.Indexes.CreateOne(new CreateIndexModel<Movie>(indexKeysDefinition));

            var indexes = _movies.Indexes.List().ToList();
            foreach (var index in indexes)
            {
                Console.WriteLine(index.ToJson());
            }
        }

        public async Task<List<Movie>> GetAsync()
        {
            _logger.LogInformation("Getting all movies from the database.");

            try
            {
                var movies = await _movies.Find(movie => true).ToListAsync();
                _logger.LogInformation("Successfully retrieved {Count} movies.", movies.Count);
                return movies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving movies.");
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<Movie> GetAsync(string id)
        {
            _logger.LogInformation("Getting movie with ID {Id} from the database.", id);

            try
            {
                var movie = await _movies.Find(movie => movie.Id == id).FirstOrDefaultAsync();
                if (movie == null)
                {
                    _logger.LogWarning("Movie with ID {Id} was not found.", id);
                }
                else
                {
                    _logger.LogInformation("Successfully retrieved movie with ID {Id}.", id);
                }

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving movie with ID {Id}.", id);
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<Movie> CreateAsync(Movie movie)
        {
            movie.Id = ObjectId.GenerateNewId().ToString();
            await _movies.InsertOneAsync(movie);
            return movie;
        }

        public async Task<Movie> UpdateAsync(string id, Movie movie)
        {
            _logger.LogInformation("Updating movie with ID {Id} in the database.", id);

            try
            {
                var result = await _movies.ReplaceOneAsync(movie => movie.Id == id, movie);
                if (result.MatchedCount == 0)
                {
                    _logger.LogWarning("Movie with ID {Id} was not found.", id);
                    return null;
                }

                _logger.LogInformation("Successfully updated movie with ID {Id}.", id);
                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating movie with ID {Id}.", id);
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task RemoveAsync(string id)
        {
            _logger.LogInformation("Removing movie with ID {Id} from the database.", id);

            try
            {
                var result = await _movies.DeleteOneAsync(movie => movie.Id == id);
                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning("Movie with ID {Id} was not found.", id);
                }
                else
                {
                    _logger.LogInformation("Successfully removed movie with ID {Id}.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing movie with ID {Id}.", id);
                throw; // Re-throw the exception after logging it
            }
        }

        // Get all indexes
        public async Task<List<BsonDocument>> GetIndexesAsync()
        {
            var indexes = await _movies.Indexes.List().ToListAsync();
            return indexes;
        }
    }
}
