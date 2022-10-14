using System;
using System.ComponentModel.DataAnnotations;

namespace _2.ExpenseManagement.Api.DTOs.Expense
{
    public class ExpenseListResponse
    {
        public IEnumerable<ExpenseListData>? Expenses { get; set; }
    }

    public class ExpenseListData
    {
        public Guid ID { get; set; }

        public Guid TypeID { get; set; }

        public string? Type { get; set; }

        public Guid CategoryID { get; set; }

        public string? Category { get; set; }

        public decimal? Cost { get; set; }

        public DateTime? Date { get; set; }

        public string? Description { get; set; }
    }
}

