using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Repositories
{
    public interface ISurveyUnitRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<UnitAppearance> GetUnitAppearanceByIdAsync(int appearanceId);
        Task<SurveyUnit> GetSurveyUnitByIdAsync(int surveyUnitId);
        Task<List<Survey>> GetSurveysByIdsAsync(List<int> surveyIds);
        Task<List<Survey>> GetSurveysBySurveyUnitIdAsync(int surveyUnitId);
        Task<List<SurveyUnit>> GetSurveyUnitsByUserIdAsync(int userId);
        Task AddSurveyUnitAsync(SurveyUnit surveyUnit);
        Task UpdateSurveyUnitAsync(SurveyUnit surveyUnit);
        Task DeleteSurveyUnitAsync(SurveyUnit surveyUnit);
        Task AddSurveyInUnitsAsync(List<SurveyInUnit> surveyInUnits);
        Task DeleteSurveyInUnitsAsync(List<SurveyInUnit> surveyInUnits);
    }
}
