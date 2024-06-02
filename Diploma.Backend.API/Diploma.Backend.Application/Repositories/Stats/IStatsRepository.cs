using Diploma.Backend.Application.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Repositories.Stats
{
    public interface IStatsRepository
    {
        Task<StatsForQuestion> GetStatsForQuestion(int questionId);
        Task<List<StatsForOption>> GetStatsForOption(int questionId);
        Task<List<StatsForGender>> GetStatsForGender(int questionId);
        Task<List<StatsForGeo>> GetStatsForGeo(int questionId);
        Task<List<StatsForLang>> GetStatsForLang(int questionId);
    }
}
