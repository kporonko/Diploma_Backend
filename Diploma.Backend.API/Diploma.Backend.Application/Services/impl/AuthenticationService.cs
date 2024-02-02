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

        public async Task<BaseResponse<LoginResponse>> Register(RegisterRequest registerRequest)
        {
            bool isEmailExists = await IsEmailExists(registerRequest.Email);
            
            if (isEmailExists)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<LoginResponse>(ErrorCodes.ExistingEmailException.ToString());
            
            User user = RegisterMapper.ConvertRegisterToUser(registerRequest);
            await AddUserToDb(user);

            return GenerateSuccessfulLoginResponse(user);
        }

        private async Task AddUserToDb(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
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
                     BaseResponseGenerator.GenerateBaseResponseByErrorMessage<User>(ErrorCodes.UnexistingEmailException.ToString()) :
                     BaseResponseGenerator.GenerateBaseResponseByErrorMessage<User>(ErrorCodes.InvalidPasswordException.ToString());
            }

            return BaseResponseGenerator.GenerateValidBaseResponseByUser<User>(user);
        }

        private async Task<bool> IsEmailExists(string email)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user != null;
        }
    }
}
