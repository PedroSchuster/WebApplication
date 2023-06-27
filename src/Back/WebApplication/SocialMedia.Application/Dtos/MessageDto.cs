using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ConnectionId { get; set; }

        public string? Body { get; set; }

        public string? DateTime { get; set; }
    }
}
