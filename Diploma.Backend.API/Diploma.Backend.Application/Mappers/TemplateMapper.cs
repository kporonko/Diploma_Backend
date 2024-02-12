using Diploma.Backend.Application.Dto.Request;
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
        /// <summary>
        /// Maps the template to template response.
        /// </summary>
        /// <param name="t">Template object to map.</param>
        /// <returns>TemplateResponse object.</returns>
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

        public static Template ConvertTemplateCreateRequestToTemplate(TemplateCreateRequest request)
        {
            return new Template
            {
                Name = request.Name,
                TemplateCode = request.TemplateCode,
                DefaultParams = request.DefaultParams,
            };
        }
    }
}
