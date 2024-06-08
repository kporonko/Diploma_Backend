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
        public static UserResponse CreateUserDataResponseWithoutSubscription(User? user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                Subscription = ConvertSubscriptionToResponse(user.Subscription)
            };
        }

        private static SubscriptionResponse ConvertSubscriptionToResponse(Subscription? subscription)
        {
            if (subscription == null)
            {
                return null;
            }
            return new SubscriptionResponse
            {
                Id = subscription.Id,
                SubscriptionId = subscription.SubscriptionId,
                DateChangeStatus = subscription.DateChangeStatus,
                DateCreate = subscription.DateCreate,
                IsActive = subscription.IsActive,
                Name = subscription.Name,
                UserId = subscription.UserId
            };
        }
    }
}
