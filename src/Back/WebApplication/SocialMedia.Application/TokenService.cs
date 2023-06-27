using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Application.Contratos;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Identity;

namespace SocialMedia.Application
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration,
                            UserManager<User> userManager,
                            IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])); // usado para criptografar e descriptografar o token
        }
        //  "TokenKey":  "super-secret-key", la no appsettings.json

        public async Task<string> CreateToken(UserUpdateDto userUpdateDto)
        {
            var user = _mapper.Map<User>(userUpdateDto);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddYears(1),
                SigningCredentials = creds  // baseado em uma chave de criptografia
            };

            var tokenHandler = new JwtSecurityTokenHandler(); // manipulador de token

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token); // serializar o token para string
        }
    }
}
