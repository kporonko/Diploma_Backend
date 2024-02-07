using Diploma.Backend.Domain.Common;

namespace Diploma.Backend.Application.Helpers
{
    public static class BaseResponseGenerator
    {
        public static BaseResponse<T> GenerateBaseResponseByErrorMessage<T>(string errorMessage)
        {
            return new BaseResponse<T>
            {
                Error = new Error
                {
                    Message = errorMessage
                }
            };
        }

        public static BaseResponse<T> GenerateValidBaseResponse<T>(T value)
        {
            return new BaseResponse<T>
            {
                Data = value,
            };
        }
    }
}
