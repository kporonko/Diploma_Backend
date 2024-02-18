using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services
{
    public interface ITemplateService
    {
        Task<BaseResponse<List<TemplateResponse>>> GetTemplates();
        Task<BaseResponse<TemplateResponse>> EditTemplate(TemplateEditRequest templateEditRequest);
        Task<BaseResponse<string>> DeleteTemplate(TemplateDeleteRequest templateEditRequest);
        Task<BaseResponse<TemplateResponse>> CreateTemplate(TemplateCreateRequest request);
    }
}
