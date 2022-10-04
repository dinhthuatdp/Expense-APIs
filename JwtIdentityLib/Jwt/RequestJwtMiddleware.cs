using System;
using JwtIdentityLib.Jwt;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CommonLib.Middlewares
{
    /// <summary>
    /// Current user login
    /// </summary>
    public class CurrentUser
    {
        public static UserModel? User;
    }

    /// <summary>
    /// Request Jwt middleware: get user from jwt in header.
    /// </summary>
    public class RequestJwtMiddleware
    {
        #region ---- Variables ----
        private readonly RequestDelegate _next;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next"></param>
        public RequestJwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        #region ---- Public methods ----
        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jwtHelpers"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context,
            JwtHelpers jwtHelpers)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var user = jwtHelpers.ValidateToken(token);
            if (user != null)
            {
                CurrentUser.User = user;
            }

            await _next(context);
        }
        #endregion
    }
}

