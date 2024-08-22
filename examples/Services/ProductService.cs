using examples.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace examples.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IOptions<MongoDBSettings> settings, ILogger<ProductService> logger)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _products = database.GetCollection<Product>(settings.Value.ProductsCollectionName);
            _logger = logger;

            // Crear un índice simple en el campo "Name"
            var indexKeysDefinition1 = Builders<Product>.IndexKeys.Ascending(product => product.Name);
            _products.Indexes.CreateOne(new CreateIndexModel<Product>(indexKeysDefinition1));

            // Crear un índice compuesto en los campos "Category" y "Price"
            var indexKeysDefinition = Builders<Product>.IndexKeys
                .Ascending(product => product.Name)
                .Ascending(product => product.Price);
            _products.Indexes.CreateOne(new CreateIndexModel<Product>(indexKeysDefinition));

            var indexes = _products.Indexes.List().ToList();

            foreach (var index in indexes)
            {
                Console.WriteLine(index.ToJson());
            }
        }

        public async Task<List<Product>> GetByNameAsync(string name)
        {
            return await _products.Find(product => product.Name == name).ToListAsync();
        }


        public async Task<List<Product>> GetAsync()
        {
            _logger.LogInformation("Getting all products from the database.");

            try
            {
                var products = await _products.Find(product => true).ToListAsync();
                _logger.LogInformation("Successfully retrieved {Count} products.", products.Count);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products.");
                throw; // Re-throw the exception after logging it
            }
        }

        public async Task<Product> GetAsync(string id) =>
            await _products.Find<Product>(product => product.Id == id).FirstOrDefaultAsync();

        public async Task<Product> CreateAsync(Product product)
        {
            product.Id = ObjectId.GenerateNewId().ToString();
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task UpdateAsync(string id, Product productIn)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var update = Builders<Product>.Update
                .Set(p => p.Name, productIn.Name)
                .Set(p => p.Description, productIn.Description)
                .Set(p => p.Price, productIn.Price);

            await _products.UpdateOneAsync(filter, update);
        }

        public async Task RemoveAsync(string id) =>
            await _products.DeleteOneAsync(product => product.Id == id);

        public async Task<List<BsonDocument>> GetIndexesAsync()
        {
            var indexes = await _products.Indexes.List().ToListAsync();
            return indexes;
        }
    }
}
