using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CommonLib.Services
{
    public class UserManagement
    {
        #region ---- Variables ----
        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="userManager"></param>
        public UserManagement(IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        #endregion

        #region ---- Public methods ----
        public async Task<IdentityUser?> GetCurrentUser()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            var currentUser = await _userManager.GetUserAsync(claimsPrincipal);
            return currentUser;
        }

        #endregion
    }
}

