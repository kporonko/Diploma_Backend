using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Repositories.Stats;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.Stats.impl
{
    public class StatsRetriever : IStatsRetriever
    {
        private readonly IStatsRepository _statsRepository;

        public StatsRetriever(IConfiguration configuration, IStatsRepository statsRepository)
        {
            _statsRepository = statsRepository;
        }

        public async Task<BaseResponse<StatsResponse>> GetStats(int questionId)
        {
            try
            {
                var response = new StatsResponse();
                response.StatsForQuestion = await _statsRepository.GetStatsForQuestion(questionId);
                response.StatsForOption = await _statsRepository.GetStatsForOption(questionId);
                response.StatsForGender = await _statsRepository.GetStatsForGender(questionId);
                response.StatsForGeo = await _statsRepository.GetStatsForGeo(questionId);
                response.StatsForLang = await _statsRepository.GetStatsForLang(questionId);

                return BaseResponseGenerator.GenerateValidBaseResponse(response);
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<StatsResponse>(ex.Message);
            }
        }

    }
}