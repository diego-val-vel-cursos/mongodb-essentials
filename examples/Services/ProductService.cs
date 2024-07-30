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

        public ProductService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _products = database.GetCollection<Product>(settings.Value.ProductsCollectionName);
        }

        public async Task<List<Product>> GetAsync() =>
            await _products.Find(product => true).ToListAsync();

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
    }
}
