﻿using Microsoft.AspNetCore.Identity;
using SocialMedia.Application.Dtos;

namespace SocialMedia.Application.Contratos
{
    public interface IAccountService
    {
        Task<bool> UserExists(string userName);

        Task<UserUpdateDto> GetUserbyUserNameAsync(string userName);

        Task<UserDto> GetUserbyIdAsync(int id);

        Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password);

        Task<UserUpdateDto> CreateAccountAsync(UserDto userDto);

        Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto);

        Task<bool> CreateUserRelation(int userId, string userToFollow);

        Task<bool> DeleteUserRelation(int userId, string following);
    }
}
