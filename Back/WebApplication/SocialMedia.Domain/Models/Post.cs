using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Domain.Models
{
    public class Post
    {
        public int Id { get; set; }

        public int? RootId { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public DateTime? Date { get; set; }

        public string? Body { get; set; }

        public int TotalLikes { get; set; } = 0;

        public int TotalComments { get; set; } = 0;

        public IEnumerable<PostComment>? PostComments { get; set; }
    }
}
