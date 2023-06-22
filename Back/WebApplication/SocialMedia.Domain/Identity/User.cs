using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Domain.Models;

namespace SocialMedia.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Bio { get; set; }

        public string? ProfilePicURL { get; set; }

        public IEnumerable<UserRelation>? UserRelations { get; set; }

        public IEnumerable<Post>? Posts { get; set; }

    }
}
