using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    public static class UnitAppearanceMapper
    {
        /// <summary>
        /// Maps the unit appearance to response.
        /// </summary>
        /// <param name="ua">UnitAppearance model to map.</param>
        /// <returns>Filled UnitAppearanceResponse.</returns>
        public static UnitAppearanceResponse MapUnitAppearanceToResponse(UnitAppearance ua)
        {
            return new UnitAppearanceResponse
            {
                Id = ua.Id,
                Name = ua.Name,
                TemplateName = ua.Template.Name,
                TemplateId = ua.Template.Id,
                Type = ua.Type.ToString(),
                Params = JsonConvert.DeserializeObject<Dictionary<string, string>>(ua.Params)
            };
        }
    }
}
