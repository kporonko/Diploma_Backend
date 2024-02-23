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
    public class SurveyUnitService : ISurveyUnitService
    {
        private readonly ApplicationContext _context;
        private IConfiguration _config;

        public SurveyUnitService(ApplicationContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<BaseResponse<SurveyUnitResponse>> CreateSurveyUnit(User userJwt, SurveyUnitCreateRequest surveyUnitCreateRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
            if (user == null)
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UserNotFound.ToString());

            var appearance = await _context.UnitAppearances.FirstOrDefaultAsync(x => x.Id == surveyUnitCreateRequest.AppearanceId);
            if (appearance == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UnitAppearanceNotFound.ToString());

            var surveys = await _context.Surveys.Where(x => surveyUnitCreateRequest.SurveyIds.Contains(x.Id)).ToListAsync();
            if (!surveys.Any())
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.SurveyNotFound.ToString());

            var model = await AddSurveyUnitModel(user, surveyUnitCreateRequest);
            return BaseResponseGenerator.GenerateValidBaseResponse(SurveyUnitMapper.MapSurveyUnitToResponse(model, surveys));
        }
        
        public async Task<BaseResponse<string>> DeleteSurveyUnit(User userJwt, SurveyUnitDeleteRequest surveyUnitCreateRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
            if (user == null)
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UserNotFound.ToString());

            var surveyUnit = await _context.SurveyUnits.FirstOrDefaultAsync(x => x.Id == surveyUnitCreateRequest.Id);
            
            await DeleteSurveyUnitModelFromDb(surveyUnit);
            return BaseResponseGenerator.GenerateValidBaseResponse("Survey unit deleted");
        }

        public async Task<BaseResponse<SurveyUnitResponse>> EditSurveyUnit(User userJwt, SurveyUnitEditRequest surveyUnitCreateRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
            if (user == null)
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UserNotFound.ToString());

            var appearance = await _context.UnitAppearances.FirstOrDefaultAsync(x => x.Id == surveyUnitCreateRequest.AppearanceId);
            if (appearance == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UnitAppearanceNotFound.ToString());

            var surveys = await _context.Surveys.Where(x => surveyUnitCreateRequest.SurveyIds.Contains(x.Id)).ToListAsync();
            if (!surveys.Any())
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.SurveyNotFound.ToString());

            var model = _context.SurveyUnits.Include(x => x.UnitSettings).Include(x => x.UnitAppearance).Include(x => x.SurveyInUnits).FirstOrDefault(x => x.Id == surveyUnitCreateRequest.Id);
            if (model == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.SurveyUnitNotFound.ToString());
            
            var response = await EditSurveyUnitModel(surveyUnitCreateRequest, model);
            return BaseResponseGenerator.GenerateValidBaseResponse(SurveyUnitMapper.MapSurveyUnitToResponse(model, surveys));
        }
        
        public async Task<BaseResponse<List<SurveyUnitResponse>>> GetSurveyUnits(User userJwt)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
            if (user == null)
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<SurveyUnitResponse>>(ErrorCodes.UserNotFound.ToString());

            await _context.Entry(user!).Collection(x => x.SurveyUnits).Query()
                .Include(x => x.SurveyInUnits).Include(x => x.UnitAppearance).Include(x => x.UnitSettings)
                .LoadAsync();

            var listSurveyUnit = user.SurveyUnits.ToList();
            var listResponses = await ConvertListDomainModelToResponse(listSurveyUnit);
            return BaseResponseGenerator.GenerateValidBaseResponse(listResponses);
        }

        private async Task<SurveyUnitResponse> EditSurveyUnitModel(SurveyUnitEditRequest surveyUnitCreateRequest, SurveyUnit surveyUnit)
        {
            surveyUnit.Name = surveyUnitCreateRequest.Name;
            surveyUnit.UnitSettings.OneSurveyTakePerDevice = surveyUnitCreateRequest.OneSurveyTakePerDevice;
            surveyUnit.UnitSettings.MaximumSurveysPerDevice = surveyUnitCreateRequest.MaximumSurveysPerDevice;
            surveyUnit.UnitSettings.HideAfterNoSurveys = surveyUnitCreateRequest.HideAfterNoSurveys;
            surveyUnit.UnitSettings.MessageAfterNoSurveys = surveyUnitCreateRequest.MessageAfterNoSurveys;
            surveyUnit.AppearanceId = surveyUnitCreateRequest.AppearanceId;

            return await UpdateSurveyInUnits(surveyUnitCreateRequest, surveyUnit);
            
        }

        private async Task<SurveyUnitResponse> UpdateSurveyInUnits(SurveyUnitEditRequest surveyUnitCreateRequest, SurveyUnit surveyUnit)
        {
            var requestSurveysIds = surveyUnitCreateRequest.SurveyIds;
            var surveyUnitActualSurveysIds = surveyUnit.SurveyInUnits.Select(x => x.SurveyId).ToList();

            //var surveyInUnitsIdsToDelete = _context.SurveysInUnits.Where(x => !requestSurveysIds.Contains(x.SurveyId) && x.Id == surveyUnit.Id).ToList().Select(x => x.Id).ToList();
            var surveysIdsToDelete = surveyUnit.SurveyInUnits
                .Select(x => x.SurveyId)
                .Except(requestSurveysIds)
                .ToList();
            
            await DeleteSurveyInUnits(surveysIdsToDelete, surveyUnit.Id);

            var surveyInUnitsIdsToAdd = requestSurveysIds.Where(x => !surveyUnitActualSurveysIds.Contains(x)).ToList();
            await CreateNewSurveyInUnits(surveyInUnitsIdsToAdd, surveyUnit.Id);
            var surveys = await _context.Surveys
                .Where(s => surveyUnit.SurveyInUnits.Select(su => su.SurveyId).Contains(s.Id))
                .ToListAsync();
            
            return SurveyUnitMapper.MapSurveyUnitToResponse(surveyUnit, surveys);
        }

        private async Task CreateNewSurveyInUnits(List<int> surveyInUnitsIdsToAdd, int surveyUnitId)
        {
            var listSurveyInUnitsToAdd = surveyInUnitsIdsToAdd.Select(x => new SurveyInUnit
            {
                SurveyId = x,
                SurveyUnitId = surveyUnitId
            }).ToList();
            await _context.SurveysInUnits.AddRangeAsync(listSurveyInUnitsToAdd);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteSurveyInUnits(List<int> surveyIdsToDelete, int surveyUnitId)
        {
            var rangeToDelete = await _context.SurveysInUnits.Where(x => surveyIdsToDelete.Contains(x.SurveyId) && x.SurveyUnitId == surveyUnitId).ToListAsync();
            _context.SurveysInUnits.RemoveRange(rangeToDelete);
        }

        private async Task<List<SurveyUnitResponse>> ConvertListDomainModelToResponse(List<SurveyUnit> listSurveyUnit)
        {
            var listResponses = new List<SurveyUnitResponse>();
            foreach (var surveyUnit in listSurveyUnit)
            {
                var surveys = await _context.Surveys.Include(x => x.SurveyInUnits)
                    .Where(s => surveyUnit.SurveyInUnits.Select(su => su.SurveyId).Contains(s.Id))
                    .ToListAsync();

                listResponses.Add(SurveyUnitMapper.MapSurveyUnitToResponse(surveyUnit, surveys));
            }

            return listResponses;
        }

        private async Task<SurveyUnit> AddSurveyUnitModel(User? user, SurveyUnitCreateRequest surveyUnitCreateRequest)
        {
            var model = new SurveyUnit
            {
                AppearanceId = surveyUnitCreateRequest.AppearanceId,
                Name = surveyUnitCreateRequest.Name,
                UserId = user.Id
            };

            //await AddSurveyUnitAsync(model);
            UpdateUnitSettingsAsync(model, surveyUnitCreateRequest);
            await AddSurveyInUnitsAsync(surveyUnitCreateRequest.SurveyIds, model);

            return model;
        }

        private async Task AddSurveyUnitAsync(SurveyUnit model)
        {
            await _context.SurveyUnits.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        private void UpdateUnitSettingsAsync(SurveyUnit model, SurveyUnitCreateRequest surveyUnitCreateRequest)
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

        private async Task AddSurveyInUnitsAsync(List<int> surveyIds, SurveyUnit surveyUnit)
        {
            var surveyInUnits = surveyIds.Select(surveyId => new SurveyInUnit
            {
                SurveyUnit = surveyUnit,
                SurveyId = surveyId
            }).ToList();

            _context.SurveysInUnits.AddRange(surveyInUnits);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteSurveyUnitModelFromDb(SurveyUnit? surveyUnit)
        {
            _context.SurveyUnits.Remove(surveyUnit);
            await _context.SaveChangesAsync();
        }

    }
}
