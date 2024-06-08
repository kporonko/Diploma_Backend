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
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }


        [HttpPost]
        [Route("Survey")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<SurveyIdResponse>>> Create([FromBody] SurveyCreateRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyService.CreateSurvey(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Created(HttpContext.GetEndpoint().DisplayName, result);
        }

        [HttpPatch]
        [Route("Survey")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<SurveyResponse>>> Update([FromBody] SurveyEditRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyService.EditSurvey(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        [HttpDelete]
        [Route("Survey")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<string>>> Delete([FromBody] SurveyDeleteRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyService.DeleteSurvey(user.Data, request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Created(HttpContext.GetEndpoint().DisplayName, result);
        }

        [HttpGet]
        [Route("Survey/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<SurveyResponse>>> SurveyById([FromRoute] int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyService.GetSurveyById(user.Data, id);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        

        [HttpGet]
        [Route("SurveysByUser")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<List<SurveyShortResponse>>>> SurveyByUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var result = await _surveyService.GetUserSurveys(user.Data);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
