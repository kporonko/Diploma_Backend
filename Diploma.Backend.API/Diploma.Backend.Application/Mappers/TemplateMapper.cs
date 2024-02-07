using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    public static class TemplateMapper
    {
        public static TemplateResponse MapTemplateToResponse(Template t)
        {
            return new TemplateResponse
            {
                Id = t.Id,
                Name = t.Name,
                TemplateCode = t.TemplateCode,
                DefaultParams = t.DefaultParams
            };
        }
    }
}
