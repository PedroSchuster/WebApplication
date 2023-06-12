using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence.Contextos;
using SocialMedia.Persistence.Contratos;

namespace SocialMedia.Persistence
{
    public class CommentPersist : GeralPersist, ICommentPersist
    {
        private readonly SocialMediaContext _context;

        public CommentPersist(SocialMediaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllCommensByPostIdAsync(int postId)
        {
            IQueryable<Comment> query = _context.Comments.Include(x => x.Comments);
            query = query.AsNoTracking().Where(x => x.PostId == postId).OrderBy(x => x.Id);

            return await query.ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int id, int postId)
        {
            IQueryable<Comment> query = _context.Comments.Include(x => x.Comments);
            query = query.AsNoTracking().Where(x => x.Id == id && x.PostId == postId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
