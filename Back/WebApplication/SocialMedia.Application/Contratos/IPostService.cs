using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence.Models;

namespace SocialMedia.Domain.Contratos
{
    public interface IPostService
    {
        Task<PostDto> AddPosts(int userId, string userName, PostDto model);

        Task<PostDto> UpdatePost(int userId, int postId, PostDto model);

        Task<bool> Remove(int userId, int postId);

        Task<PageList<PostTLDto>> GetAllPostsAsync(int userId, PageParams pageParams);

        Task<PageList<PostTLDto>> GetPostsFollowingPageAsync(int userId, PageParams pageParams);

        Task<PageList<PostTLDto>> GetPostsHomePageAsync(int userId, PageParams pageParams);

        Task<IEnumerable<PostDetailsDto>> GetAllCommentsAsync(int postId);

        Task<PostDetailsDto> GetPostByIdAsync(int postId);
    }
}
