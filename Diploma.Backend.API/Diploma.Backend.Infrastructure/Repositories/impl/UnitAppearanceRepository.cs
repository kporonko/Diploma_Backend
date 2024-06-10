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
    public class UnitAppearanceRepository : IUnitAppearanceRepository
    {
        private readonly ApplicationContext _context;

        public UnitAppearanceRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserWithUnitAppearancesAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UnitAppearances)
                .ThenInclude(ua => ua.Template)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task UpdateUnitAppearanceAsync(UnitAppearance unitAppearance)
        {
            _context.UnitAppearances.Update(unitAppearance);
            await _context.SaveChangesAsync();
        }
        
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Template> GetTemplateByIdAsync(int templateId)
        {
            return await _context.Templates.FirstOrDefaultAsync(t => t.Id == templateId);
        }

        public async Task SaveUnitAppearanceAsync(UnitAppearance unitAppearance)
        {
            _context.UnitAppearances.Add(unitAppearance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUnitAppearanceAsync(UnitAppearance unitAppearance)
        {
            _context.UnitAppearances.Remove(unitAppearance);
            await _context.SaveChangesAsync();
        }
    }
}
