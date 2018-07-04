using System;
using System.Threading.Tasks;
using ChatServerCore.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatServerCore.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserRepository userRepository;

        public ChatHub(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task SendMessage(string fromUser, string toUser, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", fromUser, message);
        }

        public async Task Register(string user, string message)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
