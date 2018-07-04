using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServerCore.Dtos;
using ChatServerCore.Repository.Interfaces;
using ChatServerCore.Repository.Model;
using Microsoft.AspNetCore.Mvc;

namespace ChatServerCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

       
        [HttpGet]
        public async Task<IEnumerable<ChatUser>> Get()
        {
            return await this.userRepository.GetAllUsers();
        }

        
        [HttpGet("{id}")]
        public async Task<ChatUser> Get(string id)
        {
            return await this.userRepository.GetUser(id) ?? new ChatUser();
        }

      
        [HttpPost]
        public void Post([FromBody] ChatUserDto newUser)
        {
            this.userRepository.AddUser(new ChatUser
            {
                Id = newUser.Id,
                UserName = newUser.UserName,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                NickName = newUser.NickName,
                Mobile = newUser.Mobile
            });
        }
       
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string value)
        {
            this.userRepository.UpdateUserDocument(id, value);
        }

      
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            this.userRepository.RemoveUser(id);
        }
    }
}