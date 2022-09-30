using Expense_Identity.Models;
using Expense_Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Identity.Controllers
{
    /// <summary>
    /// Authentication Controller.
    /// </summary>
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthenticateController : MyControllerBase
    {
        #region ----Variables----
        private readonly IAuthenticationService _authenticationService;
        #endregion

        #region ----Constructors----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authenticationService"></param>
        public AuthenticateController(
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        #endregion

        #region ----Actions----
        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginModelResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Response<LoginModelResponse>> Login([FromBody] LoginModelRequest model)
        {
            var result = await _authenticationService.Login(model);

            return result;
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterModelResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Response<RegisterModelResponse>> Register([FromBody] RegisterModelRequest model)
        {
            var result = await _authenticationService.Register(model);

            return result;
        }

        /// <summary>
        /// Register Admin user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterModelResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Response<RegisterModelResponse>> RegisterAdmin([FromBody] RegisterModelRequest model)
        {
            var result = await _authenticationService.RegisterAdmin(model);

            return result;
        }
        #endregion

        #region ----Private methods----
        #endregion
    }
}

