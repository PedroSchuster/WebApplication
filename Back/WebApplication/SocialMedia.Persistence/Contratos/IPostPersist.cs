using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Domain.Models;

namespace SocialMedia.Persistence.Contratos
{
    public interface IPostPersist : IGeralPersist
    {
        Task<IEnumerable<Post>> GetAllPostsAsync(int userId);

        Task<Post> GetPostByIdAsync(int id, int userId);

        Task<IEnumerable<PostComment>> GetAllCommentsByPostIdAsync(int postId);
    }
}
