using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Domain.Models
{
    public class UserLikedPost
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

    }
}
