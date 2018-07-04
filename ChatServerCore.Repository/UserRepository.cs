using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServerCore.Repository.Context;
using ChatServerCore.Repository.Interfaces;
using ChatServerCore.Repository.Model;
using Microsoft.Extensions.Options;

using MongoDB.Driver;
using MongoDB.Bson;

namespace UserbookAppApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext context = null;

        public UserRepository(IOptions<Settings> settings)
        {
            this.context = new UserContext(settings);
        }

        public async Task<IEnumerable<ChatUser>> GetAllUsers()
        {
            return await this.context.Users.Find(_ => true).ToListAsync();
        }

        // query after internal or internal id
        //
        public async Task<ChatUser> GetUser(string id)
        {
            ObjectId internalId = GetInternalId(id);
            return await this.context.Users
                             .Find(note => note.Id == id || note.InternalId == internalId)
                             .FirstOrDefaultAsync();

        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public async Task AddUser(ChatUser item)
        {
            await this.context.Users.InsertOneAsync(item);
        }

        public async Task<bool> RemoveUser(string id)
        {

            DeleteResult actionResult = await this.context.Users.DeleteOneAsync(
                Builders<ChatUser>.Filter.Eq("Id", id));

            return actionResult.IsAcknowledged
                   && actionResult.DeletedCount > 0;

        }

        public async Task<bool> UpdateUser(string id, string nickName)
        {
            var filter = Builders<ChatUser>.Filter.Eq(s => s.Id, id);
            var update = Builders<ChatUser>.Update
                                       .Set(s => s.NickName, nickName)
                                       .CurrentDate(s => s.UpdatedOn);


            UpdateResult actionResult = await this.context.Users.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                   && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateUser(string id, ChatUser item)
        {

            ReplaceOneResult actionResult = await this.context.Users
                                                      .ReplaceOneAsync(n => n.Id.Equals(id)
                                                                       , item
                                                                       , new UpdateOptions {IsUpsert = true});
            return actionResult.IsAcknowledged
                   && actionResult.ModifiedCount > 0;

        }

        // Demo function - full document update
        public async Task<bool> UpdateUserDocument(string id, string nickName)
        {
            var item = await GetUser(id) ?? new ChatUser();
            item.NickName = nickName;
            item.UpdatedOn = DateTime.Now;

            return await UpdateUser(id, item);
        }

        public async Task<bool> RemoveAllUsers()
        {

            DeleteResult actionResult = await this.context.Users.DeleteManyAsync(new BsonDocument());

            return actionResult.IsAcknowledged
                   && actionResult.DeletedCount > 0;

        }
    }
}
