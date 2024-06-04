using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.impl
{
    public class UnitAppearanceService : IUnitAppearanceService
    {
        private readonly IUnitAppearanceRepository _unitAppearanceRepository;

        public UnitAppearanceService(IUnitAppearanceRepository unitAppearanceRepository)
        {
            _unitAppearanceRepository = unitAppearanceRepository;
        }

        public async Task<BaseResponse<List<UnitAppearanceResponse>>> GetUnitAppearances(User userJwt)
        {
            try
            {
                var dbUser = await _unitAppearanceRepository.GetUserWithUnitAppearancesAsync(userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<UnitAppearanceResponse>>(ErrorCodes.UserNotFound.ToString());

                List<UnitAppearanceResponse> resList = FillUnitAppearanceResponseList(dbUser.UnitAppearances.Where(x => x.State).ToList());

                return BaseResponseGenerator.GenerateValidBaseResponse(resList);
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<UnitAppearanceResponse>>(ex.Message);
            }
        }

        public async Task<BaseResponse<UnitAppearanceResponse>> CreateUnitAppearance(User userJwt, UnitAppearanceCreateRequest unitAppearanceRequest)
        {
            try
            {
                var user = await _unitAppearanceRepository.GetUserByIdAsync(userJwt.Id);
                if (user == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.UserNotFound.ToString());

                var template = await _unitAppearanceRepository.GetTemplateByIdAsync(unitAppearanceRequest.TemplateId);
                if (template == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.TemplateNotFound.ToString());

                var model = FillUnitAppearanceModel(user, template, unitAppearanceRequest);
                await _unitAppearanceRepository.SaveUnitAppearanceAsync(model);
                return BaseResponseGenerator.GenerateValidBaseResponse(UnitAppearanceMapper.MapUnitAppearanceToResponse(model));
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ex.Message);
            }
        }

        public async Task<BaseResponse<UnitAppearanceResponse>> EditUnitAppearance(User userJwt, UnitAppearanceCreateRequest unitAppearanceRequest)
        {
            try
            {
                var user = await _unitAppearanceRepository.GetUserWithUnitAppearancesAsync(userJwt.Id);
                if (user == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.UserNotFound.ToString());

                var unitAppearance = user.UnitAppearances.FirstOrDefault(x => x.Id == unitAppearanceRequest.Id);
                if (unitAppearance == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.UnitAppearanceNotFound.ToString());

                var template = await _unitAppearanceRepository.GetTemplateByIdAsync(unitAppearanceRequest.TemplateId);
                if (template == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.TemplateNotFound.ToString());

                ChangeUnitAppearanceModelData(ref unitAppearance, user, template, unitAppearanceRequest);
                await _unitAppearanceRepository.UpdateUnitAppearanceAsync(unitAppearance);
                return BaseResponseGenerator.GenerateValidBaseResponse(UnitAppearanceMapper.MapUnitAppearanceToResponse(unitAppearance));
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ex.Message);
            }
        }

        private void ChangeUnitAppearanceModelData(ref UnitAppearance unitAppearance, User user, Template template, UnitAppearanceCreateRequest unitAppearanceRequest)
        {
            unitAppearance.Name = unitAppearanceRequest.Name;
            unitAppearance.Params = JsonSerializer.Serialize(unitAppearanceRequest.Params);
            unitAppearance.State = true;
            unitAppearance.Template = template;
            unitAppearance.TemplateId = template.Id;
            unitAppearance.User = user;
            unitAppearance.UserId = user.Id;
            unitAppearance.Type = Enum.Parse<AppearanceType>(unitAppearanceRequest.Type);
        }

        private UnitAppearance FillUnitAppearanceModel(User user, Template template, UnitAppearanceCreateRequest unitAppearanceRequest)
        {
            return new UnitAppearance
            {
                Name = unitAppearanceRequest.Name,
                Params = JsonSerializer.Serialize(unitAppearanceRequest.Params),
                State = true,
                Template = template,
                TemplateId = template.Id,
                User = user,
                UserId = user.Id,
                Type = Enum.Parse<AppearanceType>(unitAppearanceRequest.Type)
            };
        }

        private List<UnitAppearanceResponse> FillUnitAppearanceResponseList(List<UnitAppearance> unitAppearances)
        {
            return unitAppearances.Select(unitAppearance => new UnitAppearanceResponse
            {
                Id = unitAppearance.Id,
                Name = unitAppearance.Name,
                TemplateName = unitAppearance.Template.Name,
                Type = unitAppearance.Type.ToString(),
                Params = JsonSerializer.Deserialize<Dictionary<string, string>>(unitAppearance.Params)
            }).ToList();
        }
    }
}
