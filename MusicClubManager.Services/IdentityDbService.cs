using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MusicClubManager.Abstractions;
using MusicClubManager.Core.Models;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Services.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicClubManager.Services
{
    public class IdentityDbService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings) : IIdentityService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<ServiceResult<string>> Register(RegisterRequest registerRequest)
        {
            var user = new ApplicationUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName
            };
            var userWithSameEmail = await userManager.FindByEmailAsync(registerRequest.Email);
            if (userWithSameEmail == null)
            {
                var result = await userManager.CreateAsync(user, registerRequest.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Authorization.default_role.ToString());

                }
                return new ServiceResult<string>
                {
                    Data = $"User registered with username {user.UserName}"
                };
            }

            return new ServiceResult<string>
            {
                Messages = [new ServiceMessage { Message = "Registration failed." }]
            };
        }

        public async Task<ServiceResult<TokenResult>> GetToken(TokenRequest tokenRequest)
        {
            var user = await userManager.FindByEmailAsync(tokenRequest.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, tokenRequest.Password))
            {
                return new ServiceResult<TokenResult>
                {
                    Messages = [ new ServiceMessage { Message = "Failed to create the token."}]
                };
            }

            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new ServiceResult<TokenResult>
            {
                Data = new TokenResult
                {
                    TokenType = "Bearer",
                    AccessToken = accessToken,
                    ExpiresIn = (int)_jwtSettings.DurationInMinutes * 60
                }
            };  
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            //for (int i = 0; i < roles.Count; i++)
            //{
            //    roleClaims.Add(new Claim("roles", roles[i]));
            //}
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                //new Claim(JwtRegisteredClaimNames.Name, user.Email),
                //new Claim("uid", user.Id)

                new Claim(ClaimTypes.Name, user.FirstName)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials)
            { 
            };

            return jwtSecurityToken;
        }
    }
}
