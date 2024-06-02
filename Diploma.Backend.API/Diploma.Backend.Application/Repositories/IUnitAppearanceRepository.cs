using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Repositories
{
    public interface IUnitAppearanceRepository
    {
        Task<User> GetUserWithUnitAppearancesAsync(int userId);
        Task<User> GetUserByIdAsync(int userId);
        Task<Template> GetTemplateByIdAsync(int templateId);
        Task SaveUnitAppearanceAsync(UnitAppearance unitAppearance);
        Task UpdateUnitAppearanceAsync(UnitAppearance unitAppearance);
    }
}
