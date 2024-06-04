using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Repositories
{
    public interface ISurveyRepository
    {
        Task AddSurveyAsync(Survey survey);
        Task DeleteSurveyAsync(int surveyId);
        Task<Survey> GetSurveyByIdWithDetailsAsync(int surveyId);
        Task<User> GetUserByIdAsync(int userId);
        Task<List<Survey>> GetUserSurveysAsync(int userId);
        Task UpdateSurveyAsync(Survey survey);
        void DeleteQuestionsAndOptions(Survey survey);
        Targeting GetTargetingById(int targetingId);
    }
}
