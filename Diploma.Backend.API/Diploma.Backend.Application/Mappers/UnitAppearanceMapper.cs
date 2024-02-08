using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    public static class UnitAppearanceMapper
    {
        public static UnitAppearanceResponse MapUnitAppearanceToResponse(UnitAppearance ua)
        {
            return new UnitAppearanceResponse
            {
                Id = ua.Id,
                Name = ua.Name,
                TemplateName = ua.Template.Name,
                Type = ua.Type.ToString()
            };
        }
    }
}
