using JwtIdentityLib.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JwtIdentityLib
{
    /// <summary>
    /// Jwt Middleware.
    /// </summary>
    public class JwtMiddleware
    {
        #region ---- Variables ----
        private readonly RequestDelegate _next;
        private const string AUTHORIZATION = "Authorization";
        private const string USER = "User";
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next"></param>
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        #region ---- Public methods ----
        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="jwtHelpers"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context,
            UserManager<IdentityUser> userManager,
            JwtHelpers jwtHelpers)
        {
            var token = context.Request.Headers[AUTHORIZATION].FirstOrDefault()?.Split(" ").Last();
            var user = jwtHelpers.ValidateToken(token);
            if (user != null)
            {
                // attach user to context on successful jwt validation
                context.Items[USER] = await userManager.FindByIdAsync(user.ID.ToString());
            }

            await _next(context);
        }
        #endregion
    }
}

