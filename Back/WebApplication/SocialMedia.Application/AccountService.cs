using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Contratos;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Identity;
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

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userUpdateDto.UserName); // do jeito q eh feito n pode alterar o user name
                if (user == null) return null;

                userUpdateDto.Id = user.Id;

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
    }
}
