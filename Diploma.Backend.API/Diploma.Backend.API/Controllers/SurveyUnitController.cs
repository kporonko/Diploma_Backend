using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Diploma.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyUnitController : ControllerBase
    {
        private readonly ISurveyUnitService _surveyUnitService;

        public SurveyUnitController(ISurveyUnitService surveyUnitService)
        {
            _surveyUnitService = surveyUnitService;
        }

        [HttpPost]
        [Route("SurveyUnit")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<SurveyUnitResponse>>> Create([FromBody] SurveyUnitCreateRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyUnitService.CreateSurveyUnit(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Created(HttpContext.GetEndpoint().DisplayName, result);
        }

        [HttpPatch]
        [Route("SurveyUnit")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<SurveyUnitResponse>>> Edit([FromBody] SurveyUnitEditRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyUnitService.EditSurveyUnit(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("SurveyUnit")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<string>>> Delete([FromBody] SurveyUnitDeleteRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyUnitService.DeleteSurveyUnit(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("SurveyUnit")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<List<SurveyUnitResponse>>>> Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyUnitService.GetSurveyUnits(user.Data);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("SurveyUnit/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<SurveyUnit>>> Get([FromRoute] int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyUnitService.GetSurveyUnit(user.Data, id);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
