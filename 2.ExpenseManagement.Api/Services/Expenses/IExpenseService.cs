using System;
using _2.ExpenseManagement.Api.DTOs.Expense;
using CommonLib.DTOs.RequestModel;
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

        /// <summary>
        /// Get all expense of current user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response<ExpenseListResponse>> GetAll(ExpenseListRequest request);

        /// <summary>
        /// Get expense details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<ExpenseDetailsResponse>> Get(Guid id);

        /// <summary>
        /// Edit expense.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response<ExpenseEditResponse>> Edit(Guid id, ExpenseEditRequest request);

        /// <summary>
        /// Delete expense.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<ExpenseDeleteResponse>> Delete(Guid id);

        Task<Response<ExpenseSpentSnapshotResponse>> GetExpenseSnapshot(ExpenseSpentSnapshotRequest request);
    }
}

