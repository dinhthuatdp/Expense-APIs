using System;
using Microsoft.AspNetCore.Http;

namespace CommonLib.Middlewares
{
    public class RequestJwtMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestJwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }
    }
}

