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
    public class PostPersist : GeralPersist, IPostPersist
    {

        private readonly SocialMediaContext _context;

        public PostPersist(SocialMediaContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync(int userId)
        {
            IQueryable<Post> query = _context.Posts;
            query = query.AsNoTracking().Where(x=>x.UserId == userId).OrderBy(x=>x.Id);

            return await query.ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id, int userId)
        {
            IQueryable<Post> query = _context.Posts;
            query = query.AsNoTracking().Where(x=>x.Id == id && x.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PostComment>> GetAllCommentsByPostIdAsync(int postId)
        {
            IQueryable<PostComment> query = _context.PostComments;
            query = query.AsNoTracking().Where(x => x.PostId == postId).OrderBy(x => x.Id);

            return await query.ToListAsync();
        }
    }
}
