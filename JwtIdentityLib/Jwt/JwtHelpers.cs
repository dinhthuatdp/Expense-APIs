using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtIdentityLib.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtIdentityLib.Jwt
{
    /// <summary>
    /// Jwt Helpers.
    /// </summary>
    public class JwtHelpers
    {
        #region ---- Variables ----
        private readonly IConfiguration _configuration;
        private const string ID = "id";
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="jwtConfigModel"></param>
        /// <param name="configuration"></param>
        public JwtHelpers(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region ---- Public methods ----
        /// <summary>
        /// Get Token
        /// </summary>
        /// <param name="authClaims"></param>
        /// <returns></returns>
        public JwtSecurityToken? GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[JwtConst.SECRET]));

            var token = new JwtSecurityToken(
                issuer: _configuration[JwtConst.ISSUER],
                audience: _configuration[JwtConst.AUDIENCE],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        /// <summary>
        /// Validate token, if valid => return UserID else return null.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string? ValidateToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration[JwtConst.SECRET]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration[JwtConst.ISSUER],
                    ValidAudience = _configuration[JwtConst.AUDIENCE],
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ID)?.Value?.ToString();

                // return user id from JWT token if validation successful
                return userId;
            }
            catch (Exception ex)
            {
                // return null if validation fails
                return null;
            }
        }
        #endregion
    }
}

