using System;
namespace CommonLib.DTOs.RequestModel
{
    public class Request<T> where T : class
    {
    }

    public interface ISearch
    {
        public string? Search { get; set; }
    }

    public interface IRequestFilter<T>
        where T : class
    {
        public string? Search { get; set; }
    }

    public class FilterModel
    {
        public string? Name { get; set; }

        public object? Value { get; set; }
    }
}
