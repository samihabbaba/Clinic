using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Clinic.API.Dtos.SystemUsersDto;
using Clinic.API.Models;
using Clinic.Helpers;
using Clinic.API.DataAccess;
using Microsoft.AspNetCore.Http;

namespace Clinic.Services.Main
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme);
        Task<string> AuthenticateUser(LoginDto ar);
        Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties);
        Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties);
        Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties);
        Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _context;

        public AuthenticationService(DataContext context,
            UserManager<SystemUser> userManager,
            JwtSettings jwtSettings,
            RoleManager<IdentityRole> roleManager,
            TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<string> AuthenticateUser(LoginDto ar)
        {
            var user = await _userManager.FindByNameAsync(ar.UserName);

            // if (user == null|| user.IsDeleted)
            // {
            //     return new AuthenticationResult
            //     {
            //         Errors = new[] { "User With this username does not exist in the database" }
            //     };
            // }
            // else if(!user.IsActive || user.IsSuspended)
            // {
            //     return new AuthenticationResult
            //     {
            //         Errors = new[] { "Not Active User" }
            //     };
            // }

            var result = await _userManager.CheckPasswordAsync(user, ar.Password);

            //AuthenticationResult response = new AuthenticationResult();

            if (!result)
            {
                return "error";
                // return new AuthenticationResult
                // {
                //     Errors = new[] { "Username/password Combination are wrong" }
                // };
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }
        
        // public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        // {
        //     var validatedToken = GetPrincipalFromToken(token);

        //     if (validatedToken == null)
        //     {
        //         return new AuthenticationResult { Errors = new[] { "Invalid Token" } };
        //     }

        //     var expiryDateUnix =
        //         long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        //     var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        //         .AddSeconds(expiryDateUnix);

        //     if (expiryDateTimeUtc > DateTime.Now)
        //     {
        //         return new AuthenticationResult { Errors = new[] { "This token hasn't expired yet" } };
        //     }

        //     var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        //     var storedRefreshToken = await _context.RefreshToken.SingleOrDefaultAsync(x => x.Token == refreshToken);

        //     if (storedRefreshToken == null)
        //     {
        //         return new AuthenticationResult { Errors = new[] { "This refresh token does not exist" } };
        //     }

        //     if (DateTime.Now > storedRefreshToken.ExpiryDate)
        //     {
        //         return new AuthenticationResult { Errors = new[] { "This refresh token has expired" } };
        //     }

        //     if (storedRefreshToken.Invalidated)
        //     {
        //         return new AuthenticationResult { Errors = new[] { "This refresh token has been invalidated" } };
        //     }

        //     if (storedRefreshToken.Used)
        //     {
        //         return new AuthenticationResult { Errors = new[] { "This refresh token has been used" } };
        //     }

        //     if (storedRefreshToken.JwtId != jti)
        //     {
        //         return new AuthenticationResult { Errors = new[] { "This refresh token does not match this JWT" } };
        //     }

        //     storedRefreshToken.Used = true;
        //     _context.RefreshToken.Update(storedRefreshToken);
        //     await _context.SaveChangesAsync();

        //     var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
        //     return await GenerateAuthenticationResultForUserAsync(user);
        // }
        private async Task<string> GenerateAuthenticationResultForUserAsync(SystemUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id),
                // new Claim("name", user.Name),
                // new Claim("currency", user.Currency),
                // new Claim("symbol", user.Symbol),
                // new Claim("master", user.IsMasterAccount.ToString()),
                // new Claim("role", userRoles.FirstOrDefault())
            };



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.Add(_jwtSettings.TokenLifetime),
                // SigningCredentials =
                // new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            // var refreshToken = new RefreshToken
            // {
            //     JwtId = token.Id,
            //     StaffId = user.Id,
            //     CreationDate = DateTime.Now,
            //     ExpiryDate = DateTime.Now.AddMonths(6)
            // };
            // await _context.RefreshToken.AddAsync(refreshToken);
            // await _context.SaveChangesAsync();


            return tokenHandler.WriteToken(token);



        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }
    }
}
