using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerCore.Dtos
{
    public class ChatUserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string NickName { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;
    }
}
