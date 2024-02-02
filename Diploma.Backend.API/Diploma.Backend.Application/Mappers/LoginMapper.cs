﻿using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    public static class LoginMapper
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
    }
}
