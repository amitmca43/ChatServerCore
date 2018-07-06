using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServerCore.Dtos;
using ChatServerCore.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatServerCore.Hubs
{
    public class ChatHub : Hub
    {
        static Dictionary<string, string> userConnectionMapping = new Dictionary<string, string>();

        private readonly IUserRepository userRepository;

        public ChatHub(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        #region Overridden base class methods

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = userConnectionMapping.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            userConnectionMapping.Remove(username);

            await Clients.All.SendAsync("UserDisconnected", username);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        #endregion Overridden base class methods

        #region Methods related to User Chat
        
        /// <summary>
        /// This method is used to Register user for chat
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task Register(string username)
        {
            userConnectionMapping.Add(username, Context.ConnectionId);

            var activeUsers = await GetActiveUsers(username);
            var user = await GetUserByUserName(username);

            await Clients.Caller.SendAsync("ActiveUsers", activeUsers);

            await Clients.Others.SendAsync("UserConnected", user);
        }       

        /// <summary>
        /// This method is used to send personal chat messages
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="toUser"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendUserMessage(string fromUser, string toUser, string message)
        {
            var connection = userConnectionMapping[toUser];
            await Clients.Client(connection).SendAsync("ReceiveUserMessage", fromUser, message);
        }


        private async Task<List<ChatUserDto>> GetActiveUsers(string username)
        {

            var users = await this.userRepository.GetAllUsers();
            var activeUsers = new List<ChatUserDto>();

            foreach (var user in users)
            {
                if (userConnectionMapping.ContainsKey(user.UserName) && !user.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                {
                    activeUsers.Add(new ChatUserDto
                    {
                        UserName = user.UserName,
                        NickName = user.NickName,
                        Mobile = user.Mobile
                    });
                }
            }

            return activeUsers;
        }
        private async Task<ChatUserDto> GetUserByUserName(string username)
        {
            var user = await this.userRepository.GetUser(username);
            var chatUser = new ChatUserDto
            {
                UserName = user.UserName,
                NickName = user.NickName,
                Mobile = user.Mobile
            };

            return chatUser;
        }

        #endregion Methods related to User Chat


        #region Methods related to Group Chat

        public async Task JoinChatRoom(string user, string chatRoom)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);

            var groupMessages = new List<ChatRoomMessageDto>();

            await Clients.Group(chatRoom).SendAsync("UserJoined", user);
            await Clients.Caller.SendAsync("GroupMessages", groupMessages);
        }


        public async Task SendGroupMessage(string fromUser, string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", fromUser, message);
        }

        # endregion Methods related to Group Chat
    }
}
