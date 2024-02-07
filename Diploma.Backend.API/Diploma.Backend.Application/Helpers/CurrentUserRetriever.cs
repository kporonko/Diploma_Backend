using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Helpers
{
    public static class CurrentUserRetriever
    {
        /// <summary>
        /// Gets current user by authorizing jwt token.
        /// </summary>
        /// <returns></returns>
        public static BaseResponse<User> GetCurrentUser(ClaimsIdentity claimsIdentity)
        {
            try
            {
                if (claimsIdentity is not null)
                {
                    var userClaims = claimsIdentity.Claims;

                    var user =  new User
                    {
                        Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                        FirstName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                        LastName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                        Role = Enum.Parse<Role>(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value ?? string.Empty),
                        Id = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value),
                    };

                    return BaseResponseGenerator.GenerateValidBaseResponse(user);
                }
                else
                {
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<User>(ErrorCodes.UserNotFound.ToString());
                }
            }
            catch (Exception)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<User>(ErrorCodes.InvalidUserClaims.ToString());
            }
        }
    }
}
