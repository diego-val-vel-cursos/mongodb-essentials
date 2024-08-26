using MongoDB.Driver;
using MongoDB.Bson;
using Practice.Services.mslogs.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Practice.Services.mslogs.Services
{
    public class LogService
    {
        private readonly IMongoCollection<Log> _logs;
        private readonly ILogger<LogService> _logger;

        public LogService(IOptions<MongoDBSettings> settings, ILogger<LogService> logger)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _logs = database.GetCollection<Log>(settings.Value.UsersCollectionName);
            _logger = logger;

            // Crear un índice simple en el campo "Action"
            var indexKeysDefinition1 = Builders<Log>.IndexKeys.Ascending(log => log.Action);
            _logs.Indexes.CreateOne(new CreateIndexModel<Log>(indexKeysDefinition1));

            // Crear un índice compuesto en el campo "Timestamp"
            var indexKeysDefinition = Builders<Log>.IndexKeys
                .Ascending(log => log.Timestamp);
            _logs.Indexes.CreateOne(new CreateIndexModel<Log>(indexKeysDefinition));

            var indexes = _logs.Indexes.List().ToList();

            foreach (var index in indexes)
            {
                Console.WriteLine(index.ToJson());
            }
        }

        public async Task<List<Log>> GetAsync()
        {
            _logger.LogInformation("Getting all logs from the database.");

            try
            {
                var logs = await _logs.Find(log => true).ToListAsync();
                _logger.LogInformation("Successfully retrieved {Count} logs.", logs.Count);
                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving logs.");
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<Log> GetAsync(string id)
        {
            _logger.LogInformation("Getting log with ID {Id} from the database.", id);

            try
            {
                var log = await _logs.Find(log => log.Id == id).FirstOrDefaultAsync();

                if (log == null)
                {
                    _logger.LogWarning("Log with ID {Id} was not found.", id);
                }
                else
                {
                    _logger.LogInformation("Successfully retrieved log with ID {Id}.", id);
                }

                return log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving log with ID {Id}.", id);
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<Log> CreateAsync(Log log)
        {
            log.Id = ObjectId.GenerateNewId().ToString();
            await _logs.InsertOneAsync(log);
            _logger.LogInformation("Successfully created log with ID {Id}.", log.Id);
            return log;
        }

        public async Task<Log> UpdateAsync(string id, Log log)
        {
            _logger.LogInformation("Updating log with ID {Id} in the database.", id);

            try
            {
                var result = await _logs.ReplaceOneAsync(existingLog => existingLog.Id == id, log);
                if (result.MatchedCount == 0)
                {
                    _logger.LogWarning("Log with ID {Id} was not found.", id);
                    return null;
                }

                _logger.LogInformation("Successfully updated log with ID {Id}.", id);
                return log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating log with ID {Id}.", id);
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task RemoveAsync(string id)
        {
            _logger.LogInformation("Removing log with ID {Id} from the database.", id);

            try
            {
                var result = await _logs.DeleteOneAsync(log => log.Id == id);
                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning("Log with ID {Id} was not found.", id);
                }
                else
                {
                    _logger.LogInformation("Successfully removed log with ID {Id}.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing log with ID {Id}.", id);
                throw; // Re-throw the exception after logging it
            }
        }

        // Obtener todos los índices
        public async Task<List<BsonDocument>> GetIndexesAsync()
        {
            var indexes = await _logs.Indexes.List().ToListAsync();
            return indexes;
        }
    }
}
