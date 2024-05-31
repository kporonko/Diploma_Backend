using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsRetriever _statsService;

        public StatsController(IStatsRetriever statsService)
        {
            _statsService = statsService;
        }


        [HttpGet]
        [Route("Stats/{questionId}")]
        public async Task<ActionResult<BaseResponse<StatsResponse>>> GetStats([FromRoute] int questionId)
        {
            var result = await _statsService.GetStats(questionId);
            if (result.Error != null)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
