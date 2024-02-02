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
    public static class RegisterMapper
    {
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
