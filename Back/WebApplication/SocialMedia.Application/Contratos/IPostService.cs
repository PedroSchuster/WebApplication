using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Application.Dtos;

namespace SocialMedia.Domain.Contratos
{
    public interface IPostService
    {
        Task<PostDto> AddPosts(int userId, string userName, PostDto model);

        Task<PostDto> UpdatePost(int userId, int postId, PostDto model);

        Task<bool> Remove(int userId, int postId);

        Task<IEnumerable<PostDetailsDto>> GetAllCommentsAsync(int postId, int userId);

        Task<IEnumerable<PostTLDto>> GetAllPostsAsync(int userId);

        Task<PostDetailsDto> GetPostByIdAsync(int postId, int userId);
    }
}
