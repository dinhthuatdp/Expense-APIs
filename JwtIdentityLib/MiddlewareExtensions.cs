using System.Text;
using JwtIdentityLib.Constants;
using JwtIdentityLib.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JwtIdentityLib
{
    /// <summary>
    /// Middleware extensions.
    /// </summary>
    public static class JwtExtensions
    {
        #region ---- Public methods ----
        /// <summary>
        /// Add Jwt Identity.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddJwtIdentity(
        this WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;
            // Adding Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration[JwtConst.AUDIENCE],
                    ValidIssuer = configuration[JwtConst.ISSUER],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[JwtConst.SECRET]))
                };
            });
            builder.Services.AddTransient(x => new JwtHelpers(configuration));

            return builder;
        }

        /// <summary>
        /// Use Authentication and Authorization.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJwtIdentity(this IApplicationBuilder app)
        {
            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();

            return app;
        }
        #endregion
    }
}

