using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Domain.Models;

namespace SocialMedia.Application.Dtos
{
    public class PostTLDto
    {
        public int? Id { get; set; }

        public int? UserId { get; set; }

        public UserUpdateDto User { get; set; }

        public string? Date { get; set; }

        public string? Body { get; set; }

        public bool IsLiked { get; set; }

        public int TotalLikes { get; set; } = 0;

        public int TotalComments { get; set; } = 0;

    }
}
