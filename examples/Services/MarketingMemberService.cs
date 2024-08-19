using MongoDBEssentials.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using examples.Models;

namespace MongoDBEssentials.Services
{
    public class MarketingMemberService
    {
        private readonly IMongoCollection<MarketingMember> _marketingMembers;

        public MarketingMemberService(IOptions<MongoDBSettings> settings, ILogger<MarketingMemberService> logger)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _marketingMembers = database.GetCollection<MarketingMember>("MarketingMembers");
        }

        public async Task<List<MarketingMember>> GetAsync() =>
            await _marketingMembers.Find(member => true).ToListAsync();

        public async Task<MarketingMember?> GetAsync(string id) =>
            await _marketingMembers.Find<MarketingMember>(member => member.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(MarketingMember newMember) =>
            await _marketingMembers.InsertOneAsync(newMember);

        public async Task UpdateAsync(string id, MarketingMember updatedMember) =>
            await _marketingMembers.ReplaceOneAsync(member => member.Id == id, updatedMember);

        public async Task RemoveAsync(string id) =>
            await _marketingMembers.DeleteOneAsync(member => member.Id == id);
    }
}
