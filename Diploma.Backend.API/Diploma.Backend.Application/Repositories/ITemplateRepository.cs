using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Repositories
{
    public interface ITemplateRepository
    {
        Task<Template> GetTemplateByIdAsync(int id);
        Task<List<Template>> GetAllTemplatesAsync();
        Task AddTemplateAsync(Template template);
        Task RemoveTemplateAsync(Template template);
        Task UpdateTemplateAsync(Template template);
    }
}
