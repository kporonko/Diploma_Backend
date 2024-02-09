using Diploma.Backend.Domain.Common;

namespace Diploma.Backend.Application.Helpers
{
    public static class BaseResponseGenerator
    {
        /// <summary>
        /// Generates an error BaseResponse of T class.
        /// </summary>
        /// <typeparam name="T">Response model.</typeparam>
        /// <param name="errorMessage">Error message to show to user. Use ErrorCodes enum.</param>
        /// <returns>BaseResponse filled with Error.</returns>
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

        /// <summary>
        /// Generates a successful BaseResponse of T class.
        /// </summary>
        /// <typeparam name="T">Response model.</typeparam>
        /// <param name="value">Object of class to fill with data.</param>
        /// <returns>BaseResponse filled with valid data.</returns>
        public static BaseResponse<T> GenerateValidBaseResponse<T>(T value)
        {
            return new BaseResponse<T>
            {
                Data = value,
            };
        }
    }
}
