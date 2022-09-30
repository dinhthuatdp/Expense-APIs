using Expense_Identity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Expense_Identity.Services.Interfaces;
using Expense_Identity.Constants;
using Microsoft.Extensions.Localization;
using JwtIdentityLib.Jwt;

namespace Expense_Identity.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        #region ----Variables----
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHelpers _jwtHelpers;
        private readonly IStringLocalizer<AuthenticationService> _stringLocalizer;
        #endregion

        #region ----Constructors----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="configuration"></param>
        public AuthenticationService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IStringLocalizer<AuthenticationService> stringLocalizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _stringLocalizer = stringLocalizer;
            _jwtHelpers = new JwtHelpers(configuration);
        }
        #endregion

        #region ----Public methods----
        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<LoginModelResponse>> Login(LoginModelRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            string errorMessage;
            if (user is null)
            {
                errorMessage = _stringLocalizer[ErrorCode.USER_OR_PASSWORD_INVALID].ToString();
                return ToErrorResponse<LoginModelResponse>(ResponseStatusCode.Error,
                    errorMessage);
            }

            var isMatch = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isMatch)
            {
                errorMessage = _stringLocalizer[ErrorCode.USER_OR_PASSWORD_INVALID].ToString();
                return ToErrorResponse<LoginModelResponse>(ResponseStatusCode.Error,
                    errorMessage);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id)
                };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _jwtHelpers.GetToken(authClaims);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            var data = new LoginModelResponse
            {
                Token = tokenStr,
                Expiration = token?.ValidTo
            };

            return ToResponse(data,
                ResponseStatusCode.Success);
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<RegisterModelResponse>> Register(RegisterModelRequest model)
        {
            string errorMessage;
            var isExists = await CheckUserExists(model);
                if (isExists)
            {
                errorMessage = _stringLocalizer[ErrorCode.USER_EXISTS].ToString();
                return ToErrorResponse<RegisterModelResponse>(ResponseStatusCode.Error,
                    errorMessage);
            }
            IdentityUser user = new()
            {
                Email = model.Email,
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                errorMessage = _stringLocalizer[ErrorCode.USER_CREATION_FAILED].ToString();
                return ToErrorResponse<RegisterModelResponse>(ResponseStatusCode.Error,
                    errorMessage);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.USER))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.USER));
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.USER))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.USER);
            }

            return ToResponse(new RegisterModelResponse
            {
                IsSuccess = true
            }, ResponseStatusCode.Success);
        }

        /// <summary>
        /// Register Admin user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<RegisterModelResponse>> RegisterAdmin(RegisterModelRequest model)
        {
            string errorMessage;
            var isExists = await CheckUserExists(model);
            if (isExists)
            {
                errorMessage = _stringLocalizer[ErrorCode.USER_EXISTS].ToString();
                return ToErrorResponse<RegisterModelResponse>(ResponseStatusCode.Error,
                    errorMessage);
            }
            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                errorMessage = _stringLocalizer[ErrorCode.USER_CREATION_FAILED].ToString();
                return ToErrorResponse<RegisterModelResponse>(ResponseStatusCode.Error,
                    errorMessage);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.ADMIN))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.USER))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.USER));
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.ADMIN))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.ADMIN);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.ADMIN))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.USER);
            }

            return ToResponse(new RegisterModelResponse
            {
                IsSuccess = true
            }, ResponseStatusCode.Success);
        }
        #endregion

        #region ----Private methods----
        private async Task<bool> CheckUserExists(RegisterModelRequest model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return true;
            }
            userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}

