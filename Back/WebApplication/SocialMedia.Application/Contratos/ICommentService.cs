using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Application.Dtos;

namespace SocialMedia.Domain.Contratos
{
    public interface ICommentService
    {
        Task<CommentDto> AddComments(int userId, CommentDto model);

        Task<CommentDto> UpdateComment(int userId, int commentId, CommentDto model);

        Task<bool> Remove(int commentId, int postId);

        Task<IEnumerable<CommentDto>> GetAllCommentsAsync(int postId);

        Task<CommentDto> GetCommentByIdAsync(int commentId, int postId);
    }
}
