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
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.impl
{
    public class TargetingService : ITargetingService
    {
        private readonly ITargetingRepository _repository;

        public TargetingService(ITargetingRepository repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<TargetingCreateResponse>> CreateTargeting(User userJwt, TargetingCreateRequest targetingCreateRequest)
        {
            try
            {
                var dbUser = await _repository.GetUserByIdAsync(userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ErrorCodes.UserNotFound.ToString());

                var targeting = TargetingMapper.ConvertTargetingCreateRequestToTargeting(targetingCreateRequest, dbUser.Id);
                FillCountriesToTargeting(targeting, targetingCreateRequest.CountriesIds);
                await _repository.SaveTargetingAsync(targeting);
                var responseModel = TargetingMapper.MapTargetingToResponse(targeting);
                return BaseResponseGenerator.GenerateValidBaseResponse(responseModel);
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ex.Message);
            }
        }

        public async Task<BaseResponse<TargetingCreateResponse>> GetTargeting(User userJwt, int id)
        {
            try
            {
                var dbUser = await _repository.GetUserByIdAsync(userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ErrorCodes.UserNotFound.ToString());

                var targeting = await _repository.GetTargetingByIdAsync(id, dbUser.Id);
                if (targeting == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ErrorCodes.TargetingNotFound.ToString());

                var response = TargetingMapper.MapTargetingToResponse(targeting);
                return BaseResponseGenerator.GenerateValidBaseResponse(response);
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ex.Message);
            }
        }

        public async Task<BaseResponse<TargetingCreateResponse>> EditTargeting(User userJwt, TargetingCreateRequest request)
        {
            try
            {
                var dbUser = await _repository.GetUserByIdAsync(userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ErrorCodes.UserNotFound.ToString());

                var targeting = await _repository.GetTargetingByIdAsync(request.Id, dbUser.Id);
                if (targeting == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ErrorCodes.TargetingNotFound.ToString());

                await UpdateTargetingData(targeting, userJwt, request);
                var response = TargetingMapper.MapTargetingToResponse(targeting);
                return BaseResponseGenerator.GenerateValidBaseResponse(response);
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ex.Message);
            }
        }

        public async Task<BaseResponse<List<TargetingCreateResponse>>> GetTargetingsByUser(User userJwt)
        {
            try
            {
                var dbUser = await _repository.GetUserByIdAsync(userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<TargetingCreateResponse>>(ErrorCodes.UserNotFound.ToString());

                var targetings = await _repository.GetTargetingsByUserIdAsync(dbUser.Id);
                var response = targetings.Select(x => TargetingMapper.MapTargetingToResponse(x)).ToList();
                return BaseResponseGenerator.GenerateValidBaseResponse(response);
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<TargetingCreateResponse>>(ex.Message);
            }
        }

        public async Task<BaseResponse<string>> DeleteTargeting(User userJwt, int id)
        {
            var dbUser = await _repository.GetUserByIdAsync(userJwt.Id);
            if (dbUser == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<string>(ErrorCodes.UserNotFound.ToString());

            var targeting = await _repository.GetTargetingByIdAsync(id, dbUser.Id);
            if (targeting == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<string>(ErrorCodes.TargetingNotFound.ToString());

            await _repository.DeleteTargetingAsync(targeting);
            return BaseResponseGenerator.GenerateValidBaseResponse("Targeting deleted");
        }

        public async Task<BaseResponse<List<CountryResponse>>> GetAllCountries()
        {
            var countries = await _repository.GetAllCountriesAsync();
            return BaseResponseGenerator.GenerateValidBaseResponse(countries);
        }

        public async Task<BaseResponse<List<CountryResponse>>> GetTargetingsCountries(User userJwt, int targetingId)
        {
            var dbUser = await _repository.GetUserByIdAsync(userJwt.Id);
            if (dbUser == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<CountryResponse>>(ErrorCodes.UserNotFound.ToString());

            var targeting = await _repository.GetTargetingByIdAsync(targetingId, dbUser.Id);
            if (targeting == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<CountryResponse>>(ErrorCodes.TargetingNotFound.ToString());

            var response = targeting.CountryInTargetings.Select(x => new CountryResponse
            {
                Id = x.CountryId,
                Name = x.Country.Name
            }).ToList();

            return BaseResponseGenerator.GenerateValidBaseResponse(response);
        }

        private async Task FillCountriesToTargeting(Targeting targeting, List<int> countriesIds)
        {
            var countries = await _repository.GetCountriesByIdsAsync(countriesIds);
            targeting.CountryInTargetings = countries.Select(country => new CountryInTargeting
            {
                CountryId = country.Id,
                TargetingId = targeting.Id,
                Country = country
            }).ToList();
        }

        private async Task UpdateTargetingData(Targeting targeting, User userJwt, TargetingCreateRequest request)
        {
            targeting.Name = request.Name;
            targeting.Surveys = !request.SurveyIds.IsNullOrEmpty() ? await _repository.GetSurveysByIdsAsync(request.SurveyIds) : null;
            targeting.CountryInTargetings = !request.CountriesIds.IsNullOrEmpty() ?
                (await _repository.GetCountriesByIdsAsync(request.CountriesIds))
                    .Select(c => new CountryInTargeting { CountryId = c.Id, TargetingId = targeting.Id, Country = c })
                    .ToList()
                : new List<CountryInTargeting>();
            await _repository.SaveTargetingAsync(targeting);
        }
    }

}
