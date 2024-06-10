using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.Repositories.impl
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly ApplicationContext _context;

        public SurveyRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddSurveyAsync(Survey survey)
        {
            await _context.Surveys.AddAsync(survey);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSurveyAsync(int surveyId)
        {
            var survey = await _context.Surveys.FindAsync(surveyId);
            if (survey != null)
            {
                _context.Surveys.Remove(survey);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Survey> GetSurveyByIdWithDetailsAsync(int surveyId)
        {
            return await _context.Surveys
                .Include(s => s.Questions)
                    .ThenInclude(q => q.QuestionOptions)
                    .ThenInclude(o => o.OptionTranslations)
                .Include(s => s.Questions)
                    .ThenInclude(q => q.QuestionLine)
                    .ThenInclude(ql => ql.QuestionTranslations)
                .Include(s => s.Targeting)
                .FirstOrDefaultAsync(x => x.Id == surveyId);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<List<Survey>> GetUserSurveysAsync(int userId)
        {
            return await _context.Surveys
                .Include(x => x.Questions)
                .Include(x => x.Targeting)
                    .ThenInclude(x => x.CountryInTargetings)
                    .ThenInclude(x => x.Country)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateSurveyAsync(Survey survey)
        {
            _context.Update(survey);
            await _context.SaveChangesAsync();
        }
        
        public void DeleteQuestionsAndOptions(Survey survey)
        {
            _context.QuestionOptions.RemoveRange(survey.Questions.SelectMany(q => q.QuestionOptions));
            _context.Questions.RemoveRange(survey.Questions);
        }

        public Targeting GetTargetingById(int targetingId)
        {
            return _context.Targetings.FirstOrDefault(x => x.Id == targetingId);
        }
    }
}
