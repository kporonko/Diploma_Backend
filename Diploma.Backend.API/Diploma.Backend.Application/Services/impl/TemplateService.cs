using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.impl
{
    public class TemplateService : ITemplateService
    {
        private readonly ApplicationContext _context;
        private IConfiguration _config;

        public TemplateService(ApplicationContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<BaseResponse<List<TemplateResponse>>> GetTemplates()
        {
            var templates = await _context.Templates.ToListAsync();

            return templates == null ?
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<TemplateResponse>>(ErrorCodes.ApiCommunicationError.ToString())
                : BaseResponseGenerator.GenerateValidBaseResponse(templates.Select(t => TemplateMapper.MapTemplateToResponse(t)).ToList());
        }
    }
}
