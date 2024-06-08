using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Diploma.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetingController : ControllerBase
    {
        private readonly ITargetingService _targetingService;

        public TargetingController(ITargetingService targetingService)
        {
            _targetingService = targetingService;
        }

        [HttpPost]
        [Route("Targeting")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<TargetingCreateResponse>>> CreateTargeting([FromBody] TargetingCreateRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity); 
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _targetingService.CreateTargeting(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Created(HttpContext.GetEndpoint().DisplayName, result);
        }

        [HttpGet]
        [Route("Targeting/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<TargetingCreateResponse>>> GetTargeting([FromRoute] int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _targetingService.GetTargeting(user.Data, id);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch]
        [Route("Targeting")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<TargetingCreateResponse>>> EditTargeting([FromBody] TargetingCreateRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _targetingService.EditTargeting(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Created(HttpContext.GetEndpoint().DisplayName, result);
        }

        [HttpDelete]
        [Route("Targeting/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<string>>> DeleteTargeting([FromRoute] int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _targetingService.DeleteTargeting(user.Data, id);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("Targetings")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<TargetingCreateResponse>>> GetTargetingsByUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _targetingService.GetTargetingsByUser(user.Data);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("Countries/{targetingId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<TargetingCreateResponse>>> GetCountriesByTargetingId([FromRoute] int targetingId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _targetingService.GetTargetingsCountries(user.Data, targetingId);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("Countries")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<TargetingCreateResponse>>> GetCountries()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _targetingService.GetAllCountries();
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
