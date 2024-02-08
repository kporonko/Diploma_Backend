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
    public class UnitAppearanceService : IUnitAppearanceService
    {
        private readonly ApplicationContext _context;
        private IConfiguration _config;

        public UnitAppearanceService(ApplicationContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<Domain.Common.BaseResponse<List<UnitAppearanceResponse>>> GetUnitAppearances(User userJwt)
        {
            try
            {
                var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
                if (dbUser == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<UnitAppearanceResponse>>(ErrorCodes.UserNotFound.ToString());

                _context.Entry(dbUser).Collection(x => x.UnitAppearances).Query().Include(x => x.Template).Load();

                List<UnitAppearanceResponse> resList = FillUnitAppearanceResponseList(dbUser.UnitAppearances.Where(x => x.State).ToList());

                return BaseResponseGenerator.GenerateValidBaseResponse(resList);
            }
            catch (Exception ex)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<UnitAppearanceResponse>>(ex.Message);
            }        
        }
        
        public async Task<Domain.Common.BaseResponse<UnitAppearanceResponse>> CreateUnitAppearance(User userJwt, UnitAppearanceCreateRequest unitAppearanceRequest)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);

                if (user == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.UserNotFound.ToString());

                var template = await _context.Templates.FirstOrDefaultAsync(x => x.Id == unitAppearanceRequest.TemplateId);

                if (template == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.TemplateNotFound.ToString());

                var model = FillUnitAppearanceModel(user, template, unitAppearanceRequest);
                await SaveUnitAppearanceModel(model);
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
                var user = await _context.Users.Include(x => x.UnitAppearances).FirstOrDefaultAsync(x => x.Id == userJwt.Id);
                if (user == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.UserNotFound.ToString());

                var unitAppearance = user.UnitAppearances.FirstOrDefault(x => x.Id == unitAppearanceRequest.Id);
                if (unitAppearance == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.UnitAppearanceNotFound.ToString());

                var template = await _context.Templates.FirstOrDefaultAsync(x => x.Id == unitAppearanceRequest.TemplateId);
                if (template == null)
                    return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<UnitAppearanceResponse>(ErrorCodes.TemplateNotFound.ToString());

                ChangeUnitAppearanceModelData(ref unitAppearance, user, template, unitAppearanceRequest);
                await _context.SaveChangesAsync();
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
            unitAppearance.Params = unitAppearanceRequest.Params.ToString();
            unitAppearance.State = true;
            unitAppearance.Template = template;
            unitAppearance.TemplateId = template.Id;
            unitAppearance.User = user;
            unitAppearance.UserId = user.Id;
            unitAppearance.Type = Enum.Parse<AppearanceType>(unitAppearanceRequest.Type);
        }

        private async Task SaveUnitAppearanceModel(UnitAppearance model)
        {
            _context.UnitAppearances.Add(model);
            await _context.SaveChangesAsync();
        }

        private UnitAppearance FillUnitAppearanceModel(User user, Template template, UnitAppearanceCreateRequest unitAppearanceRequest)
        {
            return new UnitAppearance
            {
                Name = unitAppearanceRequest.Name,
                Params = unitAppearanceRequest.Params.ToString(),
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
            List<UnitAppearanceResponse> resList = new List<UnitAppearanceResponse>();
            foreach (var unitAppearance in unitAppearances)
            {
                resList.Add(new UnitAppearanceResponse
                {
                    Id = unitAppearance.Id,
                    Name = unitAppearance.Name,
                    TemplateName = unitAppearance.Template.Name,
                    Type = unitAppearance.Type.ToString(),
                });
            }

            return resList;
        }
    }
}
