using System.Security.Claims;

namespace SocialMedia.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user) // vai pegar o user atual 
        {
            return user.FindFirst(ClaimTypes.Name)?.Value; 
        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);  
        }

        /*var claims = new List<Claim>
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
            }; 
        
        isso seta os claims do nosso token
         isso aki a gente definiu la no tokenService, entao a gente vai usar isso aki pra pegar o id e o username do usuario

         */
    }
}
