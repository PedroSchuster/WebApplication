using System.Security.Claims;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
        private readonly IHostEnvironment _hostEnvironment;

        public AccountController(IAccountService accountService, ITokenService tokenService, IHostEnvironment hostEnvironment)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _hostEnvironment = hostEnvironment;
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

        [HttpPost("getusersbyfilter")]
        public async Task<IActionResult> GetUsersByFilter()
        {
            try
            {
                var filter = Request.Form["filter"].ToString();
                var users = await _accountService.GetUsersByFilterAsync(filter, User.GetUserName());
                if (users == null) return NoContent();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuários. Erro: {ex.Message} ");
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

        [HttpGet("getuserbyusername/{userName}")]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            try
            {
                var user = await _accountService.GetUserbyUserNameAsync(userName);
                if (user == null) return NoContent();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário. Erro: {ex.Message} ");
            }
        }

        [HttpGet("checkusername/{userName}")]
        public async Task<IActionResult> CheckUserName(string userName)
        {
            try
            {
                if (userName == User.GetUserName()) return Ok();

                var user = await _accountService.GetUserbyUserNameAsync(userName);
                if (user == null) return Ok();

                return Ok("Usuário já registrado");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {e.Message}");
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
                        userId = user.Id,
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
                    userId = user.Id,
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
                var user = await _accountService.GetUserbyUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário Inválido!");

                var userReturn = await _accountService.UpdateAccount(userUpdateDto);
                if (userReturn == null) return NoContent();

                return Ok(new
                {
                    userName = userReturn.UserName,
                    firstName = userReturn.FirstName,
                    userId = userReturn.Id,
                    token = _tokenService.CreateToken(userReturn).Result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar Usuário. Erro: {ex.Message} ");
            }
        }

        [HttpPut("follow")]
        public async Task<IActionResult> Follow()
        {
            try
            {
                var user = await _accountService.GetUserbyUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário Inválido!");

                var userName = Request.Form["userName"];

                 if (await _accountService.CreateUserRelation(User.GetUserId(), userName)) return Ok();

                return BadRequest("Ocorreu um erro desconhecido!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar seguir Usuário. Erro: {ex.Message} ");
            }
        }

        [HttpPut("unfollow")]
        public async Task<IActionResult> Unfollow()
        {
            try
            {
                var user = await _accountService.GetUserbyUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário Inválido!");

                var userName = Request.Form["userName"];

                if (await _accountService.DeleteUserRelation(User.GetUserId(), userName)) return Ok();

                return BadRequest("Ocorreu um erro desconhecido!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar seguir Usuário. Erro: {ex.Message} ");
            }
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UpdloadImage()
        {
            try
            {
                var user = await _accountService.GetUserbyUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário Inválido!");

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    if (user.ProfilePicURL != null) DeleteImage(user.ProfilePicURL);
                    user.ProfilePicURL = await SaveImage(file);
                }

                var returnUser = await _accountService.UpdateAccount(user);

                return Ok(returnUser);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar eventos. Erro: {ex.Message} ");
            }
        }

        [NonAction]
        private void DeleteImage(string imageURL)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Resources\\Images", imageURL);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        [NonAction]
        private async Task<string> SaveImage(IFormFile imageFile) // formFile pq o file retornado la do request eh form file
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10).ToArray()).Replace(' ', '-');
            // pega o nome do arquivo e tira os espacos e troca por -, so pega os 10 primeiros pq o nome do arquivo pode ser muito grande

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";
            // muda o nomde do arquivo pra o nome que a gente quer, com a data e a extensao do arquivo

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Resources\\Images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}
