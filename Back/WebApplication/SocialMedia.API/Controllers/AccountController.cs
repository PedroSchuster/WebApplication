using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Extensions;
using SocialMedia.Application.Contratos;
using SocialMedia.Application.Dtos;

namespace SocialMedia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName(); // isso aki eh uma extensao do ClaimsPrincipalExtensions
                var user = await _accountService.GetUserbyUserNameAsync(userName);
                if (user == null) return NoContent();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário. Erro: {ex.Message} ");
            }
        }

        [HttpGet("getuser/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _accountService.GetUserbyIdAsync(id);
                if (user == null) return NoContent();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário. Erro: {ex.Message} ");
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _accountService.UserExists(userDto.UserName)) return BadRequest("Usuário já cadastrado!"); // aki pode fazer verificaçao por email tbm

                var user = await _accountService.CreateAccountAsync(userDto);
                if (user != null)
                {
                    return Ok(new
                    {
                        userName = user.UserName,
                        firstName = user.FirstName,
                        token = _tokenService.CreateToken(user).Result
                    });
                }
                else
                {
                    return BadRequest("Erro ao cadastrar Usuário!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar registrar Usuário. Erro: {ex.Message} ");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserbyUserNameAsync(userLogin.Username);
                if (user == null) return Unauthorized("Usuário Inválido!");

                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);
                if (result == null || !result.Succeeded) return Unauthorized("Usuário Inválido!");

                return Ok(new
                {
                    userName = user.UserName,
                    firstName = user.FirstName,
                    token = _tokenService.CreateToken(user).Result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar registrar Usuário. Erro: {ex.Message} ");
            }
        }

        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                if (userUpdateDto.UserName != User.GetUserName()) return Unauthorized("Usuário Inválido!"); // isso n deixa mudar o nome de usuario

                var user = await _accountService.GetUserbyUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário Inválido!");

                var userReturn = await _accountService.UpdateAccount(userUpdateDto);
                if (userReturn == null) return NoContent();

                return Ok(new
                {
                    userName = userReturn.UserName,
                    firstName = userReturn.FirstName,
                    token = _tokenService.CreateToken(userReturn).Result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar Usuário. Erro: {ex.Message} ");
            }
        }
    }
}
