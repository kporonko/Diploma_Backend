using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    public static class TemplateMapper
    {
        /// <summary>
        /// Maps the template to template response.
        /// </summary>
        /// <param name="t">Template object to map.</param>
        /// <returns>TemplateResponse object.</returns>
        public static Template ConvertTemplateCreateRequestToTemplate(TemplateCreateRequest request)
        {
            return new Template
            {
                Name = request.Name,
                TemplateCode = request.TemplateCode,
                DefaultParams = JsonSerializer.Serialize(request.DefaultParams)
            };
        }

        public static TemplateResponse MapTemplateToResponse(Template template)
        {
            return new TemplateResponse
            {
                Id = template.Id,
                Name = template.Name,
                TemplateCode = template.TemplateCode,
                DefaultParams = JsonSerializer.Deserialize<Dictionary<string, string>>(template.DefaultParams)
            };
        }
    }
}
