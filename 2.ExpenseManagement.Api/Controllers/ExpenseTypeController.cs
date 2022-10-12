using System;
using _2.ExpenseManagement.Api.DTOs.ExpenseTypes;
using _2.ExpenseManagement.Api.Services.ExpenseTypes;
using CommonLib.DTOs.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _2.ExpenseManagement.Api.Controllers
{
    /// <summary>
    /// Expense Type controller.
    /// </summary>
    [ApiController]
    [Route("api/expensetypes")]
    public class ExpenseTypeController : MyControllerBase
    {
        #region ---- Variables ----
        private readonly IExpenseTypeService _expenseTypeService;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="expenseTypeService"></param>
        public ExpenseTypeController(ILogger<ExpenseTypeController> logger,
            IExpenseTypeService expenseTypeService)
            : base(logger)
        {
            _expenseTypeService = expenseTypeService;
        }
        #endregion

        #region ---- APIs ----
        /// <summary>
        /// Get all expense type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<ExpenseTypeListResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Response<ExpenseTypeListResponse>> GetAll()
        {
            var response = await _expenseTypeService.GetAll();
            return response;
        }
        #endregion
    }
}

