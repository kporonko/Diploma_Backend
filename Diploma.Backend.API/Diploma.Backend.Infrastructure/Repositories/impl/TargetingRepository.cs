using Diploma.Backend.Application.Dto.Response;
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
    public class TargetingRepository : ITargetingRepository
    {
        private readonly ApplicationContext _context;

        public TargetingRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<Targeting> GetTargetingByIdAsync(int targetingId, int userId)
        {
            return await _context.Targetings
                .Include(t => t.CountryInTargetings).ThenInclude(ct => ct.Country)
                .Include(t => t.Surveys)
                .FirstOrDefaultAsync(t => t.Id == targetingId && t.UserId == userId);
        }

        public async Task<List<Country>> GetCountriesByIdsAsync(List<int> countryIds)
        {
            return await _context.Countries
                .Where(c => countryIds.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<List<Survey>> GetSurveysByIdsAsync(List<int> surveyIds)
        {
            return await _context.Surveys
                .Where(s => surveyIds.Contains(s.Id))
                .ToListAsync();
        }

        public async Task<List<CountryResponse>> GetAllCountriesAsync()
        {
            return await _context.Countries
                .Select(c => new CountryResponse { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }

        public async Task SaveTargetingAsync(Targeting targeting)
        {
            if (targeting.Id == 0)
            {
                _context.Targetings.Add(targeting);
            }
            else
            {
                _context.Targetings.Update(targeting);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTargetingAsync(Targeting targeting)
        {
            _context.Targetings.Remove(targeting);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Targeting>> GetTargetingsByUserIdAsync(int userId)
        {
            return await _context.Targetings
                .Where(t => t.UserId == userId)
                .Include(t => t.CountryInTargetings).ThenInclude(ct => ct.Country)
                .Include(x => x.Surveys)
                .ToListAsync();
        }
    }
}
