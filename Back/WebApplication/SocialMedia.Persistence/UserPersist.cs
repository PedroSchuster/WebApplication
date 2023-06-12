﻿using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Identity;
using SocialMedia.Persistence;
using SocialMedia.Persistence.Contextos;
using SocialMedia.Persistence.Contratos;

namespace ProEventos.Persistence
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

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
            // esse Users sao do identity que a gente associou com o User de domain
        }


    }
}
