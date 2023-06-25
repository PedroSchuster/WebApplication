using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Identity;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence.Contextos;
using SocialMedia.Persistence.Contratos;
using SocialMedia.Persistence.Models;

namespace SocialMedia.Persistence
{
    public class PostPersist : GeralPersist, IPostPersist
    {

        private readonly SocialMediaContext _context;

        public PostPersist(SocialMediaContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<PageList<Post>> GetAllPostsAsync(int userId, PageParams pageParams)
        {
            IQueryable<Post> query = _context.Posts;
            query = query.AsNoTracking()
                .Where(x=>x.UserId == userId)
                .Where(x=>x.RootId == null || x.RootId == 0)
                .OrderByDescending(x=>x.Date);

            return await PageList<Post>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<PageList<Post>> GetPostsFollowingPageAsync(int userId, PageParams pageParams)
        {
            IQueryable<Post> query = _context.UserRelations.AsNoTracking()
                .Where(x=>x.UserId == userId)
                .Select(x=>x.Following.Posts.Where(x=>x.RootId == null || x.RootId == 0))
                .SelectMany(x=>x)
                .OrderByDescending(x=>x.Date);
            return await PageList<Post>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<PageList<Post>> GetPostsHomePageAsync(PageParams pageParams)
        {
            IQueryable<Post> query = _context.Posts.AsNoTracking()
                .Where(x=>x.RootId == null || x.RootId == 0)
                .OrderByDescending(x => x.Date);
            return await PageList<Post>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            IQueryable<Post> query = _context.Posts.AsNoTracking();
            query = query.AsNoTracking().Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PostComment>> GetAllCommentsByPostIdAsync(int postId)
        {
            IQueryable<PostComment> query = _context.PostComments.AsNoTracking();
            query = query.AsNoTracking().Where(x => x.PostId == postId).OrderBy(x => x.Id);

            return await query.ToListAsync();
        }
    }
}
