using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
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
