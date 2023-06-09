﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ConnectionId { get; set; }

        public string? Body { get; set; }

        public DateTime DateTime { get; set; }
    }
}
