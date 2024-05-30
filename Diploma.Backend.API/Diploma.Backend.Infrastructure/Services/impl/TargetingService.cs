using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.Services.impl
{
    public class TargetingService : ITargetingService
    {
        private readonly ApplicationContext _context;
        private IConfiguration _config;

        public TargetingService(ApplicationContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<BaseResponse<TargetingCreateResponse>> CreateTargeting(User userJwt, TargetingCreateRequest targetingCreateRequest)
        {
            try
            {
                var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ErrorCodes.UserNotFound.ToString());

                var targeting = TargetingMapper.ConvertTargetingCreateRequestToTargeting(targetingCreateRequest, dbUser.Id);
                SaveTargeting(targeting);
                FillCountriesToTargeting(ref targeting, targetingCreateRequest.CountriesIds);
                LoadCountriesToTargeting(ref targeting);
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
                var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ErrorCodes.UserNotFound.ToString());

                var targeting = _context.Targetings.Include(t => t.CountryInTargetings).ThenInclude(ct => ct.Country).Include(x => x.Surveys).FirstOrDefault(t => t.Id == id && t.UserId == dbUser.Id);
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
                var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<TargetingCreateResponse>(ErrorCodes.UserNotFound.ToString());

                var targeting = _context.Targetings.Include(t => t.CountryInTargetings).ThenInclude(ct => ct.Country).Include(x => x.Surveys).FirstOrDefault(t => t.Id == request.Id && t.UserId == dbUser.Id);
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
                var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<TargetingCreateResponse>>(ErrorCodes.UserNotFound.ToString());

                _context.Entry(dbUser).Collection(x => x.Targetings).Query().Include(x => x.CountryInTargetings).Load();

                var response = dbUser.Targetings.Select(x => TargetingMapper.MapTargetingToResponse(x)).ToList();
                return BaseResponseGenerator.GenerateValidBaseResponse(response);
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<TargetingCreateResponse>>(ex.Message);
            }
        }


        public async Task<BaseResponse<string>> DeleteTargeting(User userJwt, int id)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
            if (dbUser == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<string>(ErrorCodes.UserNotFound.ToString());

            _context.Entry(dbUser).Collection(x => x.Targetings).Query().Include(x => x.CountryInTargetings).Load();
            var targeting = dbUser.Targetings.FirstOrDefault(x => x.Id == id);

            if (targeting == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<string>(ErrorCodes.TargetingNotFound.ToString());

            RemoveTargeting(targeting);
            return BaseResponseGenerator.GenerateValidBaseResponse("Targeting deleted");
        }


        public Task<BaseResponse<List<CountryResponse>>> GetAllCountries()
        {
            var countries = _context.Countries.ToList();

            var response = countries.Select(x => new CountryResponse
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return Task.FromResult(BaseResponseGenerator.GenerateValidBaseResponse(response));
        }

        public async Task<BaseResponse<List<CountryResponse>>> GetTargetingsCountries(User userJwt, int targetingId)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
            if (dbUser == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<CountryResponse>>(ErrorCodes.UserNotFound.ToString());

            var targeting = await _context.Targetings.Include(x => x.CountryInTargetings).ThenInclude(x => x.Country).FirstOrDefaultAsync(x => x.Id == targetingId && x.UserId == dbUser.Id);
            if (targeting == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<CountryResponse>>(ErrorCodes.TargetingNotFound.ToString());

            var response = targeting.CountryInTargetings.Select(x => new CountryResponse
            {
                Id = x.CountryId,
                Name = x.Country.Name
            }).ToList();

            return BaseResponseGenerator.GenerateValidBaseResponse(response);
        }
        
        private void RemoveTargeting(Targeting targeting)
        {
            _context.Targetings.Remove(targeting);
            _context.SaveChanges();
        }

        private async Task UpdateTargetingData(Targeting targeting, User userJwt, TargetingCreateRequest request)
        {
            targeting.Name = request.Name;
            targeting.Surveys = !request.SurveyIds.IsNullOrEmpty() ? await RetrieveNewSurveysToTargeting(targeting, request.SurveyIds) : null;
            targeting.CountryInTargetings = !request.CountriesIds.IsNullOrEmpty() ? await RetrieveNewCountriesToTargeting(targeting, request.CountriesIds) : null;
            _context.SaveChanges();
        }
        
        private async Task<List<CountryInTargeting>> RetrieveNewCountriesToTargeting(Targeting targeting, List<int> countriesIds)
        {
            var resList = new List<CountryInTargeting>();
            var existingCountry = await _context.Countries
                .Where(s => countriesIds.Contains(s.Id))
                .ToListAsync();
            
            foreach (var country in existingCountry)
            {
                resList.Add(new CountryInTargeting
                {
                    Country = country,
                    Targeting = targeting,
                    CountryId = country.Id,
                    TargetingId = targeting.Id
                });
            }
            return resList;
        }

        private async Task<List<Survey>> RetrieveNewSurveysToTargeting(Targeting targeting, List<int>? surveyIds)
        {
            var existingSurveys = await _context.Surveys
                .Where(s => surveyIds.Contains(s.Id))
                .ToListAsync();

            return existingSurveys;
        }



        private void LoadCountriesToTargeting(ref Targeting targeting)
        {
            _context.Entry(targeting)
                    .Collection(t => t.CountryInTargetings)
                    .Query()
                    .Include(ct => ct.Country)
                    .Load();
        }

        private void FillCountriesToTargeting(ref Targeting targeting, List<int> countriesIds)
        {
            targeting.CountryInTargetings = new List<CountryInTargeting>();
            foreach (var countryId in countriesIds)
            {
                targeting.CountryInTargetings.Add(new CountryInTargeting
                {
                    CountryId = countryId,
                    TargetingId = targeting.Id
                });
            }

            _context.SaveChanges();
        }

        private void SaveTargeting(Targeting targeting)
        {
            _context.Targetings.Add(targeting);
            _context.SaveChanges();
        }
    }
}
