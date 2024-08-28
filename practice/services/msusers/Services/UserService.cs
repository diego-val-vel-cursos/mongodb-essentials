using MongoDB.Driver;
using MongoDB.Bson;
using Practice.Services.msusers.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace Practice.Services.msusers.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IOptions<MongoDBSettings> settings, ILogger<UserService> logger,
            IHttpClientFactory clientFactory
            )
        {
            
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            
            _users = database.GetCollection<User>(settings.Value.UsersCollectionName);
            _logger = logger;

            _clientFactory = clientFactory;

            // Crear un índice simple en el campo "Name"
            var indexKeysDefinition1 = Builders<User>.IndexKeys.Ascending(user => user.Email);
            _users.Indexes.CreateOne(new CreateIndexModel<User>(indexKeysDefinition1));

            // Crear un índice compuesto en los campos "Email" y "Password"
            var indexKeysDefinition = Builders<User>.IndexKeys
                .Ascending(user => user.Email)
                .Ascending(user => user.Password);
            _users.Indexes.CreateOne(new CreateIndexModel<User>(indexKeysDefinition));

            var indexes = _users.Indexes.List().ToList();

            foreach (var index in indexes)
            {
                Console.WriteLine(index.ToJson());
            }
        }

        public async Task<List<User>> GetAsync()
        {
            _logger.LogInformation("Getting all users from the database.");

            try
            {
                var users = await _users.Find(user => true).ToListAsync();
                _logger.LogInformation("Successfully retrieved {Count} users.", users.Count);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<User> GetAsync(string id)
        {
            _logger.LogInformation("Getting user with ID {Id} from the database.", id);

            try
            {
                var user = await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    _logger.LogWarning("User with ID {Id} was not found.", id);
                }
                else
                {
                    _logger.LogInformation("Successfully retrieved user with ID {Id}.", id);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user with ID {Id}.", id);
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<User> CreateAsync(User user)
        {
            user.Id = ObjectId.GenerateNewId().ToString();
            await _users.InsertOneAsync(user);
            return user;
        }


        public async Task<User> UpdateAsync(string id, User user)
        {
            _logger.LogInformation("Updating user with ID {Id} in the database.", id);

            try
            {
                var result = await _users.ReplaceOneAsync(user => user.Id == id, user);
                if (result.MatchedCount == 0)
                {
                    _logger.LogWarning("User with ID {Id} was not found.", id);
                    return null;
                }

                _logger.LogInformation("Successfully updated user with ID {Id}.", id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user with ID {Id}.", id);
                throw; // Re-throw the exception after logging it
            }
        }



        public async Task RemoveAsync(string id)
        {
            _logger.LogInformation("Removing user with ID {Id} from the database.", id);

            try
            {
                var result = await _users.DeleteOneAsync(user => user.Id == id);
                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning("User with ID {Id} was not found.", id);
                }
                else
                {
                    _logger.LogInformation("Successfully removed user with ID {Id}.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing user with ID {Id}.", id);
                throw; // Re-throw the exception after logging it
            }
        }

        // Get all indexes
        public async Task<List<BsonDocument>> GetIndexesAsync()
        {
            var indexes = await _users.Indexes.List().ToListAsync();
            return indexes;
        }

        // Login
        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _users.Find<User>(user => user.Email == email).FirstOrDefaultAsync();

            if (user == null || user.Password != password)
            {
                _logger.LogWarning("Authentication failed for email {Email}.", email);
                return null;
            }

            return user;
        }


        public async Task<User?> BuyAsync(string userId, string movieId)
        {
            var user = await _users.Find<User>(user => user.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} was not found.", userId);
                return null;
            }

            var client = _clientFactory.CreateClient("msmovies");
            var response = await client.GetAsync($"/api/movie/decrease/{movieId}");

            if (response.IsSuccessStatusCode){
                var movie = await response.Content.ReadAsStringAsync();
                Console.WriteLine(movie);
                // Update the user's movies collection
                user.Movies.Add(movieId);
                // Update the user in the database
                await _users.ReplaceOneAsync(u => u.Id == userId, user);
            }
            else{
                throw new Exception("Unable to fetch movie details.");
            }
   
            return user;
        }

    }
}
