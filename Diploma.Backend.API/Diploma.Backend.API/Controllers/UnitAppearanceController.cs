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
    }
}
