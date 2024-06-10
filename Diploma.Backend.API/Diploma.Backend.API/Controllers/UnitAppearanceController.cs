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
    public class UnitAppearanceController : ControllerBase
    {
        private readonly IUnitAppearanceService _unitAppearanceService;

        public UnitAppearanceController(IUnitAppearanceService unitAppearanceService)
        {
            _unitAppearanceService = unitAppearanceService;
        }

        [HttpGet]
        [Route("UnitAppearances")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<UnitAppearanceResponse>>> UnitAppearances()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }
            
            var result = await _unitAppearanceService.GetUnitAppearances(user.Data);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("UnitAppearance")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<UnitAppearanceResponse>>> CreateUnitAppearance([FromBody] UnitAppearanceCreateRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _unitAppearanceService.CreateUnitAppearance(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Created(HttpContext.GetEndpoint().DisplayName, result);
        }

        [HttpPatch]
        [Route("UnitAppearance")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<UnitAppearanceResponse>>> EditUnitAppearance([FromBody] UnitAppearanceCreateRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _unitAppearanceService.EditUnitAppearance(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Created(HttpContext.GetEndpoint().DisplayName, result);
        }

        [HttpDelete]
        [Route("UnitAppearance")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<UnitAppearanceResponse>>> DeleteUnitAppearance([FromBody] UnitAppearanceDeleteRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _unitAppearanceService.DeleteUnitAppearance(user.Data, request.Id);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
