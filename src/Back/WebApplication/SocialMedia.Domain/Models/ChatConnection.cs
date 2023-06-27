using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Domain.Models
{
    public class ChatConnection
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int SecondUserId { get; set; }

    }
}
