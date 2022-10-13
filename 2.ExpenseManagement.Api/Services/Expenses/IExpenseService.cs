using System;
using _2.ExpenseManagement.Api.DTOs.Expense;
using CommonLib.DTOs.ResponseModel;

namespace _2.ExpenseManagement.Api.Services.Expenses
{
    /// <summary>
    /// Expense service interface.
    /// </summary>
    public interface IExpenseService
    {
        /// <summary>
        /// Add new expense.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response<ExpenseAddResponse>> Add(ExpenseAddRequest request);
    }
}

