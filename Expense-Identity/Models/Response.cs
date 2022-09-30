namespace Expense_Identity.Models
{
    public class Response<T> where T: class
    {
        public ResponseStatus? Status { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }
    }

    public class ResponseStatus
    {
        public ResponseStatusCode StatusCode { get; set; }

        public string? Status { get; set; }
    }

    public enum ResponseStatusCode
    {
        Success = 200,
        NotFound = 404,
        Error = 400
    }
}

