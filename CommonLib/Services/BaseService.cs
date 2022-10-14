using System;
using CommonLib.DTOs.ResponseModel;
using Microsoft.Extensions.Localization;

namespace CommonLib.Services
{
    public class BaseService
    {
        #region ---- Variables ----
        public readonly IStringLocalizer _stringLocalizer;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stringLocalizer"></param>
        public BaseService(IStringLocalizer stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region ---- Public methods ----
        public Response<T> ToResponse<T>(T? data)
            where T : class
        {
            return new Response<T>
            {
                Data = data,
                Message = string.Empty,
                Status = new ResponseStatus
                {
                    Status = ResponseStatusCode.Success.ToString(),
                    StatusCode = ResponseStatusCode.Success
                }
            };
        }

        public Response<T> ToErrorResponse<T>(ResponseStatusCode? responseStatusCode,
            string? message = "")
            where T : class
        {
            message = string.IsNullOrEmpty(message) ?
                responseStatusCode?.ToString()
                : message;
            if (responseStatusCode is null)
            {
                return new Response<T>
                {
                    Data = null,
                    Message = message,
                    Status = new ResponseStatus
                    {
                        Status = ResponseStatusCode.Error.ToString(),
                        StatusCode = ResponseStatusCode.Error
                    }
                };
            }

            return new Response<T>
            {
                Data = null,
                Message = message,
                Status = new ResponseStatus
                {
                    Status = responseStatusCode.ToString(),
                    StatusCode = responseStatusCode.Value
                }
            };
        }
        #endregion
    }
}

