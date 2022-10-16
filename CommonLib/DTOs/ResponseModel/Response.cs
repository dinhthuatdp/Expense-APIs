using System;

namespace CommonLib.DTOs.ResponseModel
{
    public class Response<T> where T : class
    {
        public ResponseStatus? Status { get; set; }

        public string? Message { get; set; }

        public Pagination? Pagination { get; set; }

        public T? Data { get; set; }
    }

    public class ResponseStatus
    {
        public ResponseStatusCode StatusCode { get; set; }

        public string? Status { get; set; }
    }

    public class Pagination
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public string? NextPage { get; set; }

        public string? PreviousPage { get; set; }
    }

    public enum ResponseStatusCode
    {
        Success = 200,
        NotFound = 404,
        Error = 400,
    }
}

