using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServerCore.Repository.Context;
using ChatServerCore.Repository.Interfaces;
using ChatServerCore.Repository.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChatServerCore.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext context = null;

        public UserRepository(IOptions<Settings> settings)
        {
            this.context = new UserContext(settings);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await this.context.Users.Find(_ => true).ToListAsync();
        }

        public async Task<User> GetUser(string username)
        {
            return await this.context.Users
                             .Find(user => user.UserName == username)
                             .FirstOrDefaultAsync();

        }
        

        public async Task AddUser(User item)
        {
           await this.context.Users.InsertOneAsync(item);
        }

        public async Task<bool> RemoveUser(string username)
        {

            DeleteResult actionResult = await this.context.Users.DeleteOneAsync(
                Builders<User>.Filter.Eq("UserName", username));

            return actionResult.IsAcknowledged
                   && actionResult.DeletedCount > 0;

        }

        public async Task<bool> UpdateUser(string username, string nickName)
        {
            var filter = Builders<User>.Filter.Eq(s => s.UserName, username);
            var update = Builders<User>.Update
                                       .Set(s => s.NickName, nickName)
                                       .CurrentDate(s => s.UpdatedOn);


            UpdateResult actionResult = await this.context.Users.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                   && actionResult.ModifiedCount > 0;
        }
    }
}
