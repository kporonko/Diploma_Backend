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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.impl
{
    public class SurveyUnitService : ISurveyUnitService
    {
        private readonly ISurveyUnitRepository _repository;

        public SurveyUnitService(ISurveyUnitRepository repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<SurveyUnitResponse>> CreateSurveyUnit(User userJwt, SurveyUnitCreateRequest surveyUnitCreateRequest)
        {
            var user = await _repository.GetUserByIdAsync(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UserNotFound.ToString());

            var appearance = await _repository.GetUnitAppearanceByIdAsync(surveyUnitCreateRequest.AppearanceId);
            if (appearance == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UnitAppearanceNotFound.ToString());

            var model = await AddSurveyUnitModel(user, surveyUnitCreateRequest, appearance);
            return BaseResponseGenerator.GenerateValidBaseResponse(SurveyUnitMapper.MapSurveyUnitToResponse(model, new List<Survey>()));
        }

        public async Task<BaseResponse<string>> DeleteSurveyUnit(User userJwt, SurveyUnitDeleteRequest surveyUnitDeleteRequest)
        {
            var user = await _repository.GetUserByIdAsync(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<string>(ErrorCodes.UserNotFound.ToString());

            var surveyUnit = await _repository.GetSurveyUnitByIdAsync(surveyUnitDeleteRequest.Id);
            if (surveyUnit == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<string>(ErrorCodes.SurveyUnitNotFound.ToString());

            await _repository.DeleteSurveyUnitAsync(surveyUnit);
            return BaseResponseGenerator.GenerateValidBaseResponse("Survey unit deleted");
        }

        public async Task<BaseResponse<SurveyUnitResponse>> EditSurveyUnit(User userJwt, SurveyUnitEditRequest surveyUnitEditRequest)
        {
            var user = await _repository.GetUserByIdAsync(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UserNotFound.ToString());

            var appearance = await _repository.GetUnitAppearanceByIdAsync(surveyUnitEditRequest.AppearanceId);
            if (appearance == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UnitAppearanceNotFound.ToString());

            var surveys = await _repository.GetSurveysByIdsAsync(surveyUnitEditRequest.SurveyIds);

            var model = await _repository.GetSurveyUnitByIdAsync(surveyUnitEditRequest.Id);
            if (model == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.SurveyUnitNotFound.ToString());

            var response = await EditSurveyUnitModel(surveyUnitEditRequest, model);
            return BaseResponseGenerator.GenerateValidBaseResponse(SurveyUnitMapper.MapSurveyUnitToResponse(model, surveys));
        }

        public async Task<BaseResponse<List<SurveyUnitResponse>>> GetSurveyUnits(User userJwt)
        {
            var user = await _repository.GetUserByIdAsync(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<SurveyUnitResponse>>(ErrorCodes.UserNotFound.ToString());

            var listSurveyUnit = await _repository.GetSurveyUnitsByUserIdAsync(user.Id);
            var listResponses = await ConvertListDomainModelToResponse(listSurveyUnit);
            return BaseResponseGenerator.GenerateValidBaseResponse(listResponses);
        }

        public async Task<BaseResponse<SurveyUnit>> GetSurveyUnit(User data, int id)
        {
            var user = await _repository.GetUserByIdAsync(data.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnit>(ErrorCodes.UserNotFound.ToString());

            var unit = user.SurveyUnits.FirstOrDefault(x => x.Id == id);
            if (unit == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnit>(ErrorCodes.SurveyUnitNotFound.ToString());

            return BaseResponseGenerator.GenerateValidBaseResponse(unit);
        }

        private async Task<SurveyUnitResponse> EditSurveyUnitModel(SurveyUnitEditRequest surveyUnitEditRequest, SurveyUnit surveyUnit)
        {
            surveyUnit.Name = surveyUnitEditRequest.Name;
            surveyUnit.UnitSettings.OneSurveyTakePerDevice = surveyUnitEditRequest.OneSurveyTakePerDevice;
            surveyUnit.UnitSettings.MaximumSurveysPerDevice = surveyUnitEditRequest.MaximumSurveysPerDevice;
            surveyUnit.UnitSettings.HideAfterNoSurveys = surveyUnitEditRequest.HideAfterNoSurveys;
            surveyUnit.UnitSettings.MessageAfterNoSurveys = surveyUnitEditRequest.MessageAfterNoSurveys;
            surveyUnit.AppearanceId = surveyUnitEditRequest.AppearanceId;
            surveyUnit.UnitAppearance = surveyUnit.UnitAppearance;

            return await UpdateSurveyInUnits(surveyUnitEditRequest, surveyUnit);
        }

        private async Task<SurveyUnitResponse> UpdateSurveyInUnits(SurveyUnitEditRequest surveyUnitEditRequest, SurveyUnit surveyUnit)
        {
            var requestSurveysIds = surveyUnitEditRequest.SurveyIds ?? new List<int>();
            var surveyUnitActualSurveysIds = surveyUnit.SurveyInUnits.Select(x => x.SurveyId).ToList();

            var surveysIdsToDelete = surveyUnitActualSurveysIds
                .Except(requestSurveysIds)
                .ToList();

            var surveysInUnitsToDelete = surveyUnit.SurveyInUnits
                .Where(x => surveysIdsToDelete.Contains(x.SurveyId))
                .ToList();

            await _repository.DeleteSurveyInUnitsAsync(surveysInUnitsToDelete);

            var surveyInUnitsIdsToAdd = requestSurveysIds
                .Where(x => !surveyUnitActualSurveysIds.Contains(x))
                .ToList();

            var surveyInUnitsToAdd = surveyInUnitsIdsToAdd
                .Select(x => new SurveyInUnit
                {
                    SurveyId = x,
                    SurveyUnitId = surveyUnit.Id
                })
                .ToList();

            await _repository.AddSurveyInUnitsAsync(surveyInUnitsToAdd);

            var surveys = await _repository.GetSurveysBySurveyUnitIdAsync(surveyUnit.Id);
            return SurveyUnitMapper.MapSurveyUnitToResponse(surveyUnit, surveys);
        }

        private async Task<List<SurveyUnitResponse>> ConvertListDomainModelToResponse(List<SurveyUnit> listSurveyUnit)
        {
            var listResponses = new List<SurveyUnitResponse>();
            foreach (var surveyUnit in listSurveyUnit)
            {
                var surveys = await _repository.GetSurveysBySurveyUnitIdAsync(surveyUnit.Id);
                listResponses.Add(SurveyUnitMapper.MapSurveyUnitToResponse(surveyUnit, surveys));
            }

            return listResponses;
        }

        private async Task<SurveyUnit> AddSurveyUnitModel(User user, SurveyUnitCreateRequest surveyUnitCreateRequest, UnitAppearance unitAppearance)
        {
            var model = new SurveyUnit
            {
                AppearanceId = unitAppearance.Id,
                Name = surveyUnitCreateRequest.Name,
                UserId = user.Id,
                UnitAppearance = unitAppearance
            };

            UpdateUnitSettings(model, surveyUnitCreateRequest);
            await _repository.AddSurveyUnitAsync(model);

            return model;
        }

        private void UpdateUnitSettings(SurveyUnit model, SurveyUnitCreateRequest surveyUnitCreateRequest)
        {
            model.UnitSettings = new UnitSettings
            {
                HideAfterNoSurveys = surveyUnitCreateRequest.HideAfterNoSurveys,
                MessageAfterNoSurveys = surveyUnitCreateRequest.MessageAfterNoSurveys,
                OneSurveyTakePerDevice = surveyUnitCreateRequest.OneSurveyTakePerDevice,
                MaximumSurveysPerDevice = surveyUnitCreateRequest.MaximumSurveysPerDevice,
                UserId = model.UserId
            };
        }
    }
}
