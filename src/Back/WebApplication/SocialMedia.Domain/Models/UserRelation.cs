using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Domain.Identity;

namespace SocialMedia.Domain.Models
{
    public class UserRelation
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public int FollowingId { get; set; }

        public User? Following { get; set; }
    }
}
