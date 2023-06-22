using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Domain.Models;

namespace SocialMedia.Application.Dtos
{
    public class PostDetailsDto
    {
        public int? Id { get; set; }

        public int? UserId { get; set; }

        public int? RootId { get; set; }

        public string? UserIcon { get; set; }

        public string? UserName { get; set; }

        public string? Date { get; set; }

        public string? Body { get; set; }

        public int TotalLikes { get; set; } = 0;

        public int TotalComments { get; set; } = 0;

        public IEnumerable<PostDetailsDto>? Comments { get; set; }

        public IEnumerable<PostDetailsDto>? Parents { get; set; }

    }
}
