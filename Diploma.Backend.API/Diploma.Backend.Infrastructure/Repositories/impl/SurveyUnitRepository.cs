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
    public class SurveyUnitRepository : ISurveyUnitRepository
    {
        private readonly ApplicationContext _context;

        public SurveyUnitRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<UnitAppearance> GetUnitAppearanceByIdAsync(int appearanceId)
        {
            return await _context.UnitAppearances.FirstOrDefaultAsync(x => x.Id == appearanceId);
        }

        public async Task<SurveyUnit> GetSurveyUnitByIdAsync(int surveyUnitId)
        {
            return await _context.SurveyUnits.Include(x => x.UnitSettings)
                                             .Include(x => x.UnitAppearance)
                                             .Include(x => x.SurveyInUnits)
                                             .FirstOrDefaultAsync(x => x.Id == surveyUnitId);
        }

        public async Task<List<Survey>> GetSurveysByIdsAsync(List<int> surveyIds)
        {
            return await _context.Surveys.Where(x => surveyIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<Survey>> GetSurveysBySurveyUnitIdAsync(int surveyUnitId)
        {
            return await _context.Surveys.Where(s => s.SurveyInUnits.Any(su => su.SurveyUnitId == surveyUnitId)).ToListAsync();
        }

        public async Task<List<SurveyUnit>> GetSurveyUnitsByUserIdAsync(int userId)
        {
            var user = await _context.Users.Include(x => x.SurveyUnits)
                                           .ThenInclude(x => x.UnitAppearance)
                                           .Include(x => x.SurveyUnits)
                                           .ThenInclude(x => x.UnitSettings)
                                           .Include(x => x.SurveyUnits)
                                           .ThenInclude(x => x.SurveyInUnits)
                                           .FirstOrDefaultAsync(x => x.Id == userId);

            return user?.SurveyUnits.ToList();
        }

        public async Task AddSurveyUnitAsync(SurveyUnit surveyUnit)
        {
            await _context.SurveyUnits.AddAsync(surveyUnit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSurveyUnitAsync(SurveyUnit surveyUnit)
        {
            _context.SurveyUnits.Update(surveyUnit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSurveyUnitAsync(SurveyUnit surveyUnit)
        {
            _context.SurveyUnits.Remove(surveyUnit);
            await _context.SaveChangesAsync();
        }

        public async Task AddSurveyInUnitsAsync(List<SurveyInUnit> surveyInUnits)
        {
            await _context.SurveysInUnits.AddRangeAsync(surveyInUnits);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSurveyInUnitsAsync(List<SurveyInUnit> surveyInUnits)
        {
            _context.SurveysInUnits.RemoveRange(surveyInUnits);
            await _context.SaveChangesAsync();
        }
    }
}
