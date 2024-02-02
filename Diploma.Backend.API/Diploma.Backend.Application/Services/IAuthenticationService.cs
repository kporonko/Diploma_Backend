using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services
{
    public interface IAuthenticationService
    {
        public Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest);
        public Task<BaseResponse<LoginResponse>> Register(RegisterRequest registerRequest);
    }
}
