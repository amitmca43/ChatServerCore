using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServerCore.Dtos;
using ChatServerCore.Repository.Interfaces;
using ChatServerCore.Repository.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

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
        public async Task<ActionResult<IEnumerable<ChatUser>>> GetAll()
        {
            var users = await this.userRepository.GetAllUsers();
            return Ok(users);
        }

        
        [HttpGet("{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ChatUser>> GetById(string username)
        {
            var user = await this.userRepository.GetUser(username);
            if (user == null)
            {
                return new NotFoundResult();
            }
            return user;
        }

      
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody] ChatUserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await this.userRepository.AddUser(new ChatUser
            {
                UserName = newUser.UserName,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                NickName = newUser.NickName,
                Mobile = newUser.Mobile
            });

            var user = await this.GetById(newUser.UserName);

            return CreatedAtAction(nameof(GetById), new { userName = newUser.UserName}, user.Value);
          
        }
       
        [HttpPut("{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put(string username, [FromBody]string nickName)
        {
            var user = await this.userRepository.GetUser(username);
            if (user == null)
            {
                return new NotFoundResult();
            }

            var result = await this.userRepository.UpdateUser(username, nickName);

            return Ok();
        }

      
        [HttpDelete("{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult>  Delete(string username)
        {
            var user = await this.userRepository.GetUser(username);
            if (user == null)
            {
                return new NotFoundResult();
            }

            await this.userRepository.RemoveUser(username);

            return Ok();
        }
    }
}