using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    /// <summary>
    /// Mapper for login workflow.
    /// </summary>
    public static class LoginMapper
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
    }
}
