using System;
using _2.ExpenseManagement.Api.DTOs.ExpenseTypes;
using CommonLib.DTOs.ResponseModel;

namespace _2.ExpenseManagement.Api.Services.ExpenseTypes
{
    public interface IExpenseTypeService
    {
        Task<Response<ExpenseTypeListResponse>> GetAll();
    }
}

