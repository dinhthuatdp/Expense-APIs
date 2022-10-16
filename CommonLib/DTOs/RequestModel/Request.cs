using System;
namespace CommonLib.DTOs.RequestModel
{
    public class Request<T> where T : class
    {
        public T? Data { get; set; }
    }
}

