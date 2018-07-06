using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServerCore.Repository.Model;

namespace ChatServerCore.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUser(string username);

        // add new User document
        Task AddUser(User item);

        // remove a single document / User
        Task<bool> RemoveUser(string username);

        // update just a single document / User
        Task<bool> UpdateUser(string username, string nickName);
    }
}
