using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Repositories
{
    public interface ITargetingRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<Targeting> GetTargetingByIdAsync(int targetingId, int userId);
        Task<List<Country>> GetCountriesByIdsAsync(List<int> countryIds);
        Task<List<Survey>> GetSurveysByIdsAsync(List<int> surveyIds);
        Task<List<CountryResponse>> GetAllCountriesAsync();
        Task SaveTargetingAsync(Targeting targeting);
        Task DeleteTargetingAsync(Targeting targeting);
        Task<List<Targeting>> GetTargetingsByUserIdAsync(int userId);
    }

}
