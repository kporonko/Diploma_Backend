using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
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
    }
}
