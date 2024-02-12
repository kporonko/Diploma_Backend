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
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet]
        [Route("Templates")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BaseResponse<List<TemplateResponse>>>> Templates()
        {
            var result = await _templateService.GetTemplates();
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("Template")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse<TemplateResponse>>> Delete([FromBody] TemplateDeleteRequest request)
        {
            var result = await _templateService.DeleteTemplate(request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch]
        [Route("Template")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse<TemplateResponse>>> Edit([FromBody] TemplateEditRequest request)
        {
            var result = await _templateService.EditTemplate(request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("Template")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse<TemplateResponse>>> Create([FromBody] TemplateCreateRequest request)
        {
            var result = await _templateService.CreateTemplate(request);
            if (result.Error != null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
