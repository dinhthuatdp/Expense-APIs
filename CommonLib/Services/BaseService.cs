using System;
using CommonLib.DTOs.RequestModel;
using CommonLib.DTOs.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace CommonLib.Services
{
    public class BaseService
    {
        #region ---- Variables ----
        public readonly IStringLocalizer _stringLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stringLocalizer"></param>
        public BaseService(IStringLocalizer stringLocalizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _stringLocalizer = stringLocalizer;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region ---- Public methods ----
        /// <summary>
        /// Create response with pagination info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<T> ToPagedResponse<T>(int total,
            T data,
            RequestPagination? request)
            where T : class
        {
            if (request is null)
            {
                return new Response<T>
                {
                    Data = null,
                    Pagination = new Pagination
                    {
                        PageNumber = 0,
                        PageSize = 0,
                        TotalPages = 0
                    },
                    Message = "Bad request.",
                    Status = new ResponseStatus
                    {
                        Status = ResponseStatusCode.Error.ToString(),
                        StatusCode = ResponseStatusCode.Error
                    }
                };
            }
            var host = _httpContextAccessor.HttpContext.Request.Host;
            var path = _httpContextAccessor.HttpContext.Request.Path;
            var schema = _httpContextAccessor.HttpContext.Request.Scheme;

            if (data is null)
            {
                return new Response<T>
                {
                    Data = null,
                    Pagination = new Pagination
                    {
                        PageNumber = request == null ? 0 : request.PageNumber,
                        PageSize = request == null ? 0 : request.PageSize,
                        TotalPages = 0
                    },
                    Status = new ResponseStatus
                    {
                        Status = ResponseStatusCode.Success.ToString(),
                        StatusCode = ResponseStatusCode.Success
                    }
                };
            }
            int? nextPageNumber = (request.PageNumber + 1) * request.PageSize - total <= request.PageSize
                ? request.PageNumber + 1
                : null;
            int? prevPageNumber;
            if (request.PageNumber <= 1)
            {
                prevPageNumber = null;
            }
            else
            {
                prevPageNumber = request.PageNumber - 1;
            }

            return new Response<T>
            {
                Data = data,
                Message = string.Empty,
                Pagination = new Pagination
                {
                    PageNumber = request == null ? 0 : request.PageNumber,
                    PageSize = request == null ? 0 : request.PageSize,
                    TotalPages = total,
                    NextPage = nextPageNumber == null ? null
                        : $"{schema}://{host}{path}?pageNumber={nextPageNumber}&pageSize={request?.PageSize}",
                    PreviousPage = prevPageNumber == null ? null
                        : $"{schema}://{host}{path}?pageNumber={prevPageNumber}&pageSize={request?.PageSize}"
                },
                Status = new ResponseStatus
                {
                    Status = ResponseStatusCode.Success.ToString(),
                    StatusCode = ResponseStatusCode.Success
                }
            };
        }

        /// <summary>
        /// Create response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public Response<T> ToResponse<T>(T? data)
            where T : class
        {
            return new Response<T>
            {
                Data = data,
                Pagination = null,
                Message = string.Empty,
                Status = new ResponseStatus
                {
                    Status = ResponseStatusCode.Success.ToString(),
                    StatusCode = ResponseStatusCode.Success
                }
            };
        }

        /// <summary>
        /// Create error response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseStatusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
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

