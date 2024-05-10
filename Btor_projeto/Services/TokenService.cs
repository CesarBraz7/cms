using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Btor_projeto.Dtos;

namespace Btor_projeto
{
    public class TokenService
    {
       public Token GenerateToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);
            var credentialsAccess = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
           
            var tokenAccess = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user, "access"),
                SigningCredentials = credentialsAccess,
                Expires = DateTime.UtcNow.AddHours(2),
            };
            
            var acessToken = handler.CreateToken(tokenAccess);
            
            var token = new Token
            {
                userId = user.Id,
                Access = handler.WriteToken(acessToken),
                Role = user.Role,
            };
            
            return token; 
        }

       public string GenerateFpToken(User user)
       {
           var handler = new JwtSecurityTokenHandler();
           
           var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);
           var credentialsAccess = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
           
           var tokenDescriptor = new SecurityTokenDescriptor
           {
               Subject = GenerateClaims(user, "access"),
               SigningCredentials = credentialsAccess,
               Expires = DateTime.UtcNow.AddMinutes(15),
           };
       
           var token = handler.CreateToken(tokenDescriptor);
       
           return handler.WriteToken(token);
       }

        private static ClaimsIdentity GenerateClaims(User user, string tokenType)
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("user_id", user.Id.ToString()));
            ci.AddClaim(new Claim("token_type", tokenType));
            ci.AddClaim(new Claim(ClaimTypes.Role, user.Role));
            
            return ci;
        }
    }
}
