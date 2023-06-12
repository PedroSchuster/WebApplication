using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Domain.Models;

namespace SocialMedia.Persistence.Contratos
{
    public interface ICommentPersist : IGeralPersist
    {
        Task<IEnumerable<Comment>> GetAllCommensByPostIdAsync(int postId);

        Task<Comment> GetCommentByIdAsync(int id, int postId);
    }
}
