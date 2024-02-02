using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Extensions;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationContext _context;
        private IConfiguration _config;

        public AuthenticationService(ApplicationContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {
            BaseResponse<User> currUser = await GetUser(loginRequest);

            return currUser.Data is not null
                    ? GenerateSuccessfulLoginResponse(currUser.Data)
                    : BaseResponseGenerator.GenerateBaseResponseByErrorMessage<LoginResponse>(currUser.Error.Message);
        }

        private BaseResponse<LoginResponse> GenerateSuccessfulLoginResponse(User user)
        {
            var token = UserExtensions.GenerateTokenFromUser(user, _config["Jwt:Key"], _config["Jwt:Issuer"], _config["Jwt:Audience"]);
            LoginResponse loginResponse = LoginMapper.CreateTokenResponse(token);
            return BaseResponseGenerator.GenerateValidBaseResponseByUser<LoginResponse>(loginResponse);
        }

        private async Task<BaseResponse<User>> GetUser(LoginRequest loginRequest)
        {
            var passwordHash = loginRequest.Password.ConvertPasswordToHash();
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

            bool isPasswordMatch = passwordHash == user?.Password;
            if (!isPasswordMatch)
            {
                return user is null ? 
                     BaseResponseGenerator.GenerateBaseResponseByErrorMessage<User>(ErrorCodes.InvalidEmailException) :
                     BaseResponseGenerator.GenerateBaseResponseByErrorMessage<User>(ErrorCodes.InvalidPasswordException);
            }

            return BaseResponseGenerator.GenerateValidBaseResponseByUser<User>(user);
        }
    }
}
