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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.Services.impl
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

        /// <summary>
        /// Gets all user`s unit appearances from User entity.
        /// </summary>
        /// <param name="userJwt">User model taken from api token whose unit appearance to retrieve.</param>
        /// <returns>BaseResponse filled with Data if everythings fine. Filled with Error if there was an error.</returns>
        public async Task<BaseResponse<List<UnitAppearanceResponse>>> GetUnitAppearances(User userJwt)
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

        /// <summary>
        /// Creates UnitAppearance entity from User model and UnitAppearanceRequest.
        /// </summary>
        /// <param name="userJwt">User model taken from api token.</param>
        /// <param name="unitAppearanceRequest">UnitAppearance request.</param>
        /// <returns>BaseResponse filled with Data if everythings fine. Filled with Error if there was an error.</returns>
        public async Task<BaseResponse<UnitAppearanceResponse>> CreateUnitAppearance(User userJwt, UnitAppearanceCreateRequest unitAppearanceRequest)
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

        /// <summary>
        /// Updates UnitAppearance entity from User model and UnitAppearanceRequest.
        /// </summary>
        /// <param name="userJwt">User model taken from api token.</param>
        /// <param name="unitAppearanceRequest">UnitAppearance request with Id.</param>
        /// <returns>BaseResponse filled with Data if everythings fine. Filled with Error if there was an error.</returns>
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

        /// <summary>
        /// Updates UnitAppearance entity with new data from User, Template entities and UnitAppearanceRequest model.
        /// </summary>
        /// <param name="unitAppearance">UnitAppearance to update.</param>
        /// <param name="user">User model whose UA is being changed.</param>
        /// <param name="template">Template of UA.</param>
        /// <param name="unitAppearanceRequest">UnitAppearanceCreate request filled with new actual data.</param>
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

        /// <summary>
        /// Saves UnitAppearance model to database.
        /// </summary>
        /// <param name="model">UnitAppearance model to save.</param>
        private async Task SaveUnitAppearanceModel(UnitAppearance model)
        {
            _context.UnitAppearances.Add(model);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Fills UnitAppearance model with values from User, Template and UnitAppearanceCreateRequest.
        /// </summary>
        /// <param name="user">User whose UA is being created.</param>
        /// <param name="template">Template that is used for UA creation.</param>
        /// <param name="unitAppearanceRequest">UnitAppearanceCreateRequest filled with data of UA to create.</param>
        /// <returns>UnitAppearance model ready for saving.</returns>
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

        /// <summary>
        /// Fills list of UA to list of UnitAppearanceResponse.
        /// </summary>
        /// <param name="unitAppearances">UA list to convert to response.</param>
        /// <returns>List of UnitAppearanceResponse.</returns>
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
                    Params = JsonSerializer.Deserialize<Dictionary<string, string>>(unitAppearance.Params)
                });
            }

            return resList;
        }
    }
}
