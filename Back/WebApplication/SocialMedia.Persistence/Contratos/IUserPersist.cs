using SocialMedia.Domain.Identity;
using SocialMedia.Domain.Models;

namespace SocialMedia.Persistence.Contratos
{
    public interface IUserPersist : IGeralPersist
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<IEnumerable<User>> GetUsersByFilterAsync(string filter, string loggedUserName);

        Task<User> GetUserByIdAsync(int id);

        Task<User> GetUserByUserNameAsync(string userName);

        Task<UserRelation> GetUserRelation(int userId, int followingId);

        Task<IEnumerable<User>> GetFollowers(int id);
        
        Task<IEnumerable<User>> GetFollowing(int id);
    }
}
