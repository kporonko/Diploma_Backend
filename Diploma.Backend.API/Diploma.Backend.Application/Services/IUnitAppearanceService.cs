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
    public interface IUnitAppearanceService
    {
        Task<BaseResponse<List<UnitAppearanceResponse>>> GetUnitAppearances(User userJwt);
        Task<BaseResponse<UnitAppearanceResponse>> CreateUnitAppearance(User userJwt, UnitAppearanceCreateRequest unitAppearanceRequest);
        Task<BaseResponse<UnitAppearanceResponse>> EditUnitAppearance(User userJwt, UnitAppearanceCreateRequest unitAppearanceRequest);
        Task<BaseResponse<string>> DeleteUnitAppearance(User userJwt, int unitAppearanceId);
    }
}
