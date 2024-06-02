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
    public class TemplateRepository : ITemplateRepository
    {
        private readonly ApplicationContext _context;

        public TemplateRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Template> GetTemplateByIdAsync(int id)
        {
            return await _context.Templates.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Template>> GetAllTemplatesAsync()
        {
            return await _context.Templates.ToListAsync();
        }

        public async Task AddTemplateAsync(Template template)
        {
            await _context.Templates.AddAsync(template);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTemplateAsync(Template template)
        {
            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTemplateAsync(Template template)
        {
            _context.Templates.Update(template);
            await _context.SaveChangesAsync();
        }
    }
}
