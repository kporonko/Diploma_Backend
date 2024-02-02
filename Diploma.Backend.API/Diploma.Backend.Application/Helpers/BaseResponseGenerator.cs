using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Helpers
{
    public static class BaseResponseGenerator
    {
        public static BaseResponse<T> GenerateBaseResponseByErrorMessage<T>(ErrorCodes errorCode)
        {
            return new BaseResponse<T>
            {
                Error = new Error
                {
                    Message = errorCode
                }
            };
        }

        public static BaseResponse<T> GenerateValidBaseResponseByUser<T>(T value)
        {
            return new BaseResponse<T>
            {
                Data = value,
            };
        }
    }
}
