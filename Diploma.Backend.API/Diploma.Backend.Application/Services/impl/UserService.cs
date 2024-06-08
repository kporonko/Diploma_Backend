using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<UserResponse>> GetUserData(User userJwt)
        {
            var user = await _userRepository.GetUserWithSubscription(userJwt.Id);
            if (user == null)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UserResponse>(ErrorCodes.UserNotFound.ToString());
            }
            return BaseResponseGenerator.GenerateValidBaseResponse(UserMapper.CreateUserDataResponseWithoutSubscription(user));
        }
    }
}
