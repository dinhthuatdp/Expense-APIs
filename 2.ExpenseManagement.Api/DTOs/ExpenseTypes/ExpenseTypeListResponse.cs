using System;
namespace _2.ExpenseManagement.Api.DTOs.ExpenseTypes
{
    /// <summary>
    /// Expense type list response
    /// </summary>
    public class ExpenseTypeListResponse
    {
        public List<ExpenseTypeData>? ExpenseTypes { get; set; }
    }

    public class ExpenseTypeData
    {
        public Guid ID { get; set; }

        public string? Name { get; set; }
    }
}

