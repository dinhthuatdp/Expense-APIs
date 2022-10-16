using System;
using _2.ExpenseManagement.Api.DTOs.Expense;
using _2.ExpenseManagement.Api.Services.Expenses;
using CommonLib.DTOs.RequestModel;
using CommonLib.DTOs.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _2.ExpenseManagement.Api.Controllers
{
    /// <summary>
    /// Expense Controller.
    /// </summary>
    [ApiController]
    [Route("api/expenses")]
    public class ExpenseController : MyControllerBase
    {
        #region ---- Variables ----
        private readonly IExpenseService _expenseService;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="expenseService"></param>
        public ExpenseController(ILogger<ExpenseController> logger,
            IExpenseService expenseService)
            : base(logger)
        {
            _expenseService = expenseService;
        }
        #endregion

        #region ---- APIs ----
        /// <summary>
        /// Add new expense.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<ExpenseAddResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Response<ExpenseAddResponse>> Add([FromForm] ExpenseAddRequest request)
        {
            var response = await _expenseService.Add(request);

            return response;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<ExpenseListResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Response<ExpenseListResponse>> GetAll([FromQuery] ExpenseListRequest request)
        {
            var response = await _expenseService.GetAll(request);

            return response;
        }
        #endregion
    }
}

