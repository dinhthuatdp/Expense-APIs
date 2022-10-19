using System;
namespace _2.ExpenseManagement.Api.DTOs.Expense
{
    public class ExpenseSpentSnapshotResponse
    {
        public decimal SoFarThisMonth { get; set; }

        public decimal Today { get; set; }

        public decimal Yesterday { get; set; }

        public IEnumerable<CommonData>? CommonData { get; set; }
    }

    public class CommonData
    {
        public decimal PassAverage { get; set; }

        public decimal ThisMonth { get; set; }

        public decimal SpentExtra { get; set; }

        public string? CategoryName { get; set; }
    }
}

