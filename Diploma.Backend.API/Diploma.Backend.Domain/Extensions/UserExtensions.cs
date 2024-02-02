using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Diploma.Backend.Domain.Extensions
{
    public static class UserExtensions
    {
        /// <summary>
        /// Method generates a jwt token depending on user data.
        /// </summary>
        /// <param name="user">User who wants to receive token.</param>
        /// <param name="jwtKey">Jwt key from config.</param>
        /// <param name="issuer">Jwt issuer from config.</param>
        /// <param name="audience">Jwt audience from config.</param>
        /// <returns>Jwt token string.</returns>
        public static string GenerateTokenFromUser(this User user, string jwtKey, string issuer, string audience)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
