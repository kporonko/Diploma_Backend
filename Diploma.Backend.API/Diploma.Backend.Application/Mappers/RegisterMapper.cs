using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Extensions;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    /// <summary>
    /// Mapper for register workflow.
    /// </summary>
    public static class RegisterMapper
    {
        /// <summary>
        /// Creates the LoginResponse object from access token.
        /// </summary>
        /// <param name="token">Access token.</param>
        /// <returns>Login response object filled with access token.</returns>
        public static LoginResponse CreateTokenResponse(string token)
        {
            return new LoginResponse
            {
                AccessToken = new AccessToken
                {
                    Token = token
                }
            };
        }

        /// <summary>
        /// Converts the register request to user entity.
        /// </summary>
        /// <param name="registerRequest">User register data.</param>
        /// <returns>User entity filled with register data.</returns>
        public static User ConvertRegisterToUser(RegisterRequest registerRequest)
        {
            return new User
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Password = registerRequest.Password.ConvertPasswordToHash(),
                Role = Domain.Enums.Role.User
            };
        }
    }
}
