using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.impl
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _repository;

        public TemplateService(ITemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<TemplateResponse>> CreateTemplate(TemplateCreateRequest request)
        {
            var response = TemplateMapper.ConvertTemplateCreateRequestToTemplate(request);
            await _repository.AddTemplateAsync(response);
            return BaseResponseGenerator.GenerateValidBaseResponse(TemplateMapper.MapTemplateToResponse(response));
        }

        public async Task<BaseResponse<string>> DeleteTemplate(TemplateDeleteRequest templateDeleteRequest)
        {
            var response = await _repository.GetTemplateByIdAsync(templateDeleteRequest.Id);
            if (response == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<string>(ErrorCodes.TemplateNotFound.ToString());

            await _repository.RemoveTemplateAsync(response);
            return BaseResponseGenerator.GenerateValidBaseResponse("Template deleted");
        }

        public async Task<BaseResponse<TemplateResponse>> EditTemplate(TemplateEditRequest templateEditRequest)
        {
            var response = await _repository.GetTemplateByIdAsync(templateEditRequest.Id);
            if (response == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TemplateResponse>(ErrorCodes.TemplateNotFound.ToString());

            response.Name = templateEditRequest.Name;
            response.DefaultParams = JsonConvert.SerializeObject(templateEditRequest.DefaultParams);
            await _repository.UpdateTemplateAsync(response);
            return BaseResponseGenerator.GenerateValidBaseResponse(TemplateMapper.MapTemplateToResponse(response));
        }

        public async Task<BaseResponse<List<TemplateResponse>>> GetTemplates()
        {
            var templates = await _repository.GetAllTemplatesAsync();
            return templates == null ?
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<TemplateResponse>>(ErrorCodes.ApiCommunicationError.ToString())
                : BaseResponseGenerator.GenerateValidBaseResponse(templates.Select(t => TemplateMapper.MapTemplateToResponse(t)).ToList());
        }
    }
}
