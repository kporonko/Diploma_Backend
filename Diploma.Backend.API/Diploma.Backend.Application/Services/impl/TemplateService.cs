using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
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

        public async Task<BaseResponse<TemplateResponse>> CreateTemplate(TemplateCreateRequest request)
        {
            var response = TemplateMapper.ConvertTemplateCreateRequestToTemplate(request);
            await AddTemplateToDb(response);
            return BaseResponseGenerator.GenerateValidBaseResponse(TemplateMapper.MapTemplateToResponse(response));
        }
        
        public async Task<BaseResponse<string>> DeleteTemplate(TemplateDeleteRequest templateDeleteRequest)
        {
            var response = await _context.Templates.FirstOrDefaultAsync(x => x.Id == templateDeleteRequest.Id);
            if (response == null)
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TemplateResponse>(ErrorCodes.TemplateNotFound.ToString());
            
            await RemoveTemplateFromDb(response);
            return BaseResponseGenerator.GenerateValidBaseResponse("Template deleted");
        }
        
        public async Task<BaseResponse<TemplateResponse>> EditTemplate(TemplateEditRequest templateEditRequest)
        {
            var response = await _context.Templates.FirstOrDefaultAsync(x => x.Id == templateEditRequest.Id);
            if (response == null)
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TemplateResponse>(ErrorCodes.TemplateNotFound.ToString());

            await UpdateTemplate(response, templateEditRequest);
            return BaseResponseGenerator.GenerateValidBaseResponse(TemplateMapper.MapTemplateToResponse(response));
        }

        public async Task<BaseResponse<List<TemplateResponse>>> GetTemplates()
        {
            var templates = await _context.Templates.ToListAsync();

            return templates == null ?
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<TemplateResponse>>(ErrorCodes.ApiCommunicationError.ToString())
                : BaseResponseGenerator.GenerateValidBaseResponse(templates.Select(t => TemplateMapper.MapTemplateToResponse(t)).ToList());
        }
        private async Task RemoveTemplateFromDb(Template? response)
        {
            _context.Templates.Remove(response);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateTemplate(Template? response, TemplateEditRequest templateEditRequest)
        {
            response.Name = templateEditRequest.Name;
            //_context.Templates.Update(response);
            await _context.SaveChangesAsync();
        }


        private async Task AddTemplateToDb(Template response)
        {
            await _context.Templates.AddAsync(response);
            await _context.SaveChangesAsync();
        }
    }
}
