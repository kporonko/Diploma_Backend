using Azure.Core;
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
    public interface ITargetingService
    {
        Task<BaseResponse<TargetingCreateResponse>> CreateTargeting(User userJwt, TargetingCreateRequest targetingCreateRequest);
        Task<BaseResponse<TargetingCreateResponse>> EditTargeting(User userJwt, TargetingCreateRequest request);
        Task<BaseResponse<TargetingCreateResponse>> GetTargeting(User userJwt, int id);
        Task<BaseResponse<List<CountryResponse>>> GetAllCountries();
        Task<BaseResponse<List<CountryResponse>>> GetTargetingsCountries(User userJwt, int targetingId);
        Task<BaseResponse<List<TargetingCreateResponse>>> GetTargetingsByUser(User userJwt);
        Task<BaseResponse<string>> DeleteTargeting(User userJwt, int id);
    }
}
