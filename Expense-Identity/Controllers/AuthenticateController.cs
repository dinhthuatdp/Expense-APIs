using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expense_Identity.Constants;
using Expense_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Expense_Identity.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        #region ----Variables----
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        #endregion

        #region ----Constructors----
        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        #endregion

        #region ----Actions----
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            Response result = null;
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user is null)
            {
                result = new Response
                {
                    Message = "User name or password not found",
                    Status = "Error"
                };
                return Unauthorized(result);
            }

            var isMatch = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isMatch)
            {
                result = new Response
                {
                    Message = "User name or password not found.",
                    Status = "Error"
                };
                return Unauthorized(result);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new Response
            {
                Message = null,
                Status = "Ok",
                Data = new
                {
                    token = tokenStr,
                    expiration = token.ValidTo
                }
            });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var isExists = await CheckUserExists(model);
            if (isExists)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = "User already exists!"
                    });
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response {
                        Status = "Error",
                        Message = "User creation failed! Please check user details and try again."
                    });
            }
            return Ok(new Response
            {
                Status = "Success",
                Message = "User created successfully!"
            });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var isExists = await CheckUserExists(model);
            if (isExists)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = "User already exists!"
                    });
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = "User creation failed! Please check user details and try again."
                    });
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            return Ok(new Response
            {
                Status = "Success",
                Message = "User created successfully!"
            });
        }
        #endregion

        #region ----Private methods----

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private async Task<bool> CheckUserExists(RegisterModel model)
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

