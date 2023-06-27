using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Contratos;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Identity;
using SocialMedia.Domain.Models;
using SocialMedia.Persistence.Contratos;

namespace SocialMedia.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _userPersist;

        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IMapper mapper,
                              IUserPersist userPersist)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userPersist = userPersist;
        }


        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                // usuarios do tipo User, setado ali cima
                var user = await  _userManager.Users.SingleOrDefaultAsync(u => u.UserName.ToLower() == userUpdateDto.UserName.ToLower());

                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar verificar password. Erro: " + ex.Message);
            }   
        }

        public async Task<UserUpdateDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);
                // mapea o user para o userDto com os restantes dos campos preenchidos
                if (result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserUpdateDto>(user);
                    return userToReturn;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar criar usuário. Erro: " + ex.Message);
            }
        }

        public async Task<UserUpdateDto> GetUserbyUserNameAsync(string userName)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userName);
                if (user == null) return null;

                var userUpdateDto = _mapper.Map<UserUpdateDto>(user);

                var following = await _userPersist.GetFollowing(user.Id);
                userUpdateDto.Following = _mapper.Map<IEnumerable<UserUpdateDto>>(following);
                
                userUpdateDto.FollowingCount = following.Count() > 0? following.Count() : 0;

                var followers = await _userPersist.GetFollowers(user.Id);
                userUpdateDto.Followers = _mapper.Map<IEnumerable<UserUpdateDto>>(followers);
                userUpdateDto.FollowersCount = followers.Count() > 0? followers.Count() : 0;   

                return userUpdateDto;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar pegar usuário por username. Erro: " + ex.Message);
            }
        }

        public async Task<UserDto> GetUserbyIdAsync(int id)
        {
            try
            {
                var user = await _userPersist.GetUserByIdAsync(id);
                if (user == null) return null;

                var userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar pegar usuário por username. Erro: " + ex.Message);
            }
        }

        public async Task<IEnumerable<UserUpdateDto>> GetUsersByFilterAsync(string filter, string loggedUserName)
        {
            try
            {
                var users = await _userPersist.GetUsersByFilterAsync(filter, loggedUserName);
                if (users == null || users.Count() == 0) return null;

                return _mapper.Map<IEnumerable<UserUpdateDto>>(users);

            } 
            catch (Exception ex)
            {
                
                throw new Exception("Erro ao tentar pegar usuários por filtro. Erro: " + ex.Message);
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByIdAsync(userUpdateDto.Id); 
                if (user == null) return null;

                // a imagem ja foi atualiazada em outro lugar
                if (userUpdateDto.ProfilePicURL == null || userUpdateDto.ProfilePicURL == "")
                {
                    userUpdateDto.ProfilePicURL = user.ProfilePicURL;
                }

                _mapper.Map(userUpdateDto, user);

                // UserUpdateDto vai vir com a nova senha, dai a gente gera um token e reseta a senha e dps atualiza o user
                // Precisa do token para resetar a senha, pois sem esse token de reset o usuario iria deslogar pois o token estaria diferente do que ele tem
                if (userUpdateDto.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                }

                _userPersist.Update<User>(user);

                // aki so ta pegando de volta o user que foi atualizado para retornar o userUpdateDto
                if (await _userPersist.SaveChangesAsync())
                {
                    var userToReturn =  await _userPersist.GetUserByUserNameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDto>(userToReturn);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar atualizar usuário. Erro: " + ex.Message);
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao tentar verificar se usuário existe. Erro: " + ex.Message);
            }
        }

        public async Task<bool> CreateUserRelation(int userId, string userToFollow)
        {
            try
            {
                var user = await _userPersist.GetUserByIdAsync(userId);
                if (user == null) return false;

                var userToFollowObj = await _userPersist.GetUserByUserNameAsync(userToFollow);
                if (userToFollowObj == null) return false;

                var userRelationCheck = await _userPersist.GetUserRelation(userId, userToFollowObj.Id);
                if (userRelationCheck != null) return false;

                var userRelation = new UserRelation
                {
                    User = user,
                    UserId = userId,
                    Following = userToFollowObj,
                    FollowingId = userToFollowObj.Id
                };

                _userPersist.Add<UserRelation>(userRelation);

                return await _userPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar verificar se usuário existe. Erro: " + ex.Message);
            }
        }

        public async Task<bool> DeleteUserRelation(int userId, string following)
        {
            try
            {
                var user = await _userPersist.GetUserByIdAsync(userId);
                if (user == null) return false;

                var followingObj = await _userPersist.GetUserByUserNameAsync(following);
                if (followingObj == null) return false;

                var userRelation = await _userPersist.GetUserRelation(userId, followingObj.Id);

                _userPersist.Delete(userRelation);

                return await _userPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar verificar se usuário existe. Erro: " + ex.Message);
            }
        }
    }
}
