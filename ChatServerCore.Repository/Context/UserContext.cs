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
            var indexKeys = Builders<ChatUser>.IndexKeys.Ascending(item => item.UserName);
            var indexModel = new CreateIndexModel<ChatUser>(indexKeys);
            this.database.GetCollection<ChatUser>("User").Indexes.CreateOne(indexModel);
        }

        public IMongoCollection<ChatUser> Users => this.database.GetCollection<ChatUser>("User");
    }
}
