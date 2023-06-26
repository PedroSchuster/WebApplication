using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence.Models;

namespace SocialMedia.Persistence.Contratos
{
    public interface IPostPersist : IGeralPersist
    {
        Task<PageList<Post>> GetAllPostsAsync(int userId, PageParams pageParams);

        Task<PageList<Post>> GetPostsHomePageAsync(PageParams pageParams);

        Task<PageList<Post>> GetPostsFollowingPageAsync(int userId, PageParams pageParams);

        Task<Post> GetPostByIdAsync(int id);

        Task<IEnumerable<PostComment>> GetAllCommentsByPostIdAsync(int postId);

        Task<UserLikedPost> LikePost(int userId, int postId);
    }
}
