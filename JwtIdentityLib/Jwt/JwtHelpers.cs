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
        #endregion
    }
}

