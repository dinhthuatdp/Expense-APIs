using System;
using CommonLib.DTOs.ResponseModel;

namespace CommonLib.Services
{
    public class BaseService
    {
        public Response<T> ToResponse<T>(T? data,
            ResponseStatusCode? responseStatusCode,
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
            if (responseStatusCode != ResponseStatusCode.Success)
            {
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

            return new Response<T>
            {
                Data = data,
                Message = string.Empty,
                Status = new ResponseStatus
                {
                    Status = responseStatusCode.ToString(),
                    StatusCode = responseStatusCode.Value
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
    }
}

