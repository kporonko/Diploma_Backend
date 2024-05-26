using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    public static class UserMapper
    {
        public static UserResponse CreateUserDataResponse(Subscription subscription)
        {
            return new UserResponse
            {
                Id = subscription.User.Id,
                Email = subscription.User.Email,
                FirstName = subscription.User.FirstName,
                LastName = subscription.User.LastName,
                Role = subscription.User.Role.ToString(),
                Subscription = subscription
            };
        }
    }
}
