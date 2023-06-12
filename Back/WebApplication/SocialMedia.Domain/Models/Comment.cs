﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public Post? Post { get; set; }

        public DateTime Date { get; set; }

        public string? Body { get; set; }

        public IEnumerable<Comment>? Comments { get; set; }

    }
}