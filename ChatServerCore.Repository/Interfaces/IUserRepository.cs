using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServerCore.Repository.Model;

namespace ChatServerCore.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ChatUser>> GetAllUsers();

        Task<ChatUser> GetUser(string username);

        // add new User document
        Task AddUser(ChatUser item);

        // remove a single document / User
        Task<bool> RemoveUser(string username);

        // update just a single document / User
        Task<bool> UpdateUser(string username, string nickName);
    }
}
