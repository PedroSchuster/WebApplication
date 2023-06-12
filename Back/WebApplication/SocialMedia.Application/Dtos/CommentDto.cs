using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Domain.Models;

namespace SocialMedia.Application.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public string? Date { get; set; }

        public string? Body { get; set; }

        public IEnumerable<Comment>? Comments { get; set; }
    }
}
