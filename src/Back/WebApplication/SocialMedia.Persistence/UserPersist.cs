using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Identity;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence;
using SocialMedia.Persistence.Contextos;
using SocialMedia.Persistence.Contratos;

namespace SocialMedia.Persistence
{
    public class UserPersist : GeralPersist, IUserPersist
    {
        private readonly SocialMediaContext _context;

        public UserPersist(SocialMediaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByFilterAsync(string filter, string loggedUserName)
        {
            if (filter == string.Empty) return null;
            return await _context.Users
                .Where(x=>x.UserName != loggedUserName &&
                 x.UserName.ToLower().StartsWith(filter.ToLower())).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserRelation> GetUserRelation(int userId, int followingId)
        {
            return await _context.UserRelations.Where(x => x.UserId == userId && x.FollowingId == followingId).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
            // esse Users sao do identity que a gente associou com o User de domain
        }

        public async Task<IEnumerable<User>> GetFollowing(int id)
        {
            return await _context.UserRelations.Where(x => x.UserId == id).Select(x=>x.Following).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetFollowers(int id)
        {
            return await _context.UserRelations.Where(x => x.FollowingId == id).Select(x => x.User).ToListAsync();
        }
    }
}
