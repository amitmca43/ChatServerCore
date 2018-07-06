using ChatServerCore.Repository.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatServerCore.Repository.Context
{
    public class UserContext
    {
        private readonly IMongoDatabase database = null;

        public UserContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            this.database = client.GetDatabase(settings.Value.Database);

            //Setup Index
            var indexKeys = Builders<User>.IndexKeys.Ascending(item => item.UserName);
            var indexModel = new CreateIndexModel<User>(indexKeys);
            this.database.GetCollection<User>("User").Indexes.CreateOne(indexModel);
        }

        public IMongoCollection<User> Users => this.database.GetCollection<User>("User");
    }
}
