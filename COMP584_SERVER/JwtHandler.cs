using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WorldModel;

namespace COMP584_SERVER
{
    public class JwtHandler(UserManager<WorldModelUser> userManager, IConfiguration configuration)
    {
        public async Task<JwtSecurityToken> GenerateTokenAsync(WorldModelUser user)
        {
            JwtSecurityToken token =  new (
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtSettings:ExpiryInMinutes"])),
                claims : await GetClaimsAsync(user),
                signingCredentials : GetSigningCredentials()
            );
            return token;

        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Convert.FromBase64String(configuration["JwtSettings:SecretKey"]!);
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(key);
            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsAsync(WorldModelUser user)
        {
            List<Claim> claims = [new Claim(ClaimTypes.Name, user.UserName!)];
            //claims.AddRange(await userManager.GetRolesAsync(user).Select(role => new Claim(ClaimTypes.Role, role)));
            foreach (var role in await userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
