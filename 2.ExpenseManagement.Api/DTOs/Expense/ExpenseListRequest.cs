using System;
using CommonLib.DTOs.RequestModel;

namespace _2.ExpenseManagement.Api.DTOs.Expense
{
    public class ExpenseListRequest : RequestPagination, ISearch
    {
        public string? Search { get; set; }
    }
}

