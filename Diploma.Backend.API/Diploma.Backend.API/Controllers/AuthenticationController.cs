using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Diploma.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            var result = await _authenticationService.Login(request);
            if (result.Error != null)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Register([FromBody] RegisterRequest request)
        {
            var result = await _authenticationService.Register(request);
            if (result.Error != null)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
