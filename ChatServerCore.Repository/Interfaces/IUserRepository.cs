using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServerCore.Repository.Model;

namespace ChatServerCore.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ChatUser>> GetAllUsers();

        Task<ChatUser> GetUser(string id);

        // add new User document
        Task AddUser(ChatUser item);

        // remove a single document / User
        Task<bool> RemoveUser(string id);

        // update just a single document / User
        Task<bool> UpdateUser(string id, string body);

        // demo interface - full document update
        Task<bool> UpdateUserDocument(string id, string body);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllUsers();
    }
}
