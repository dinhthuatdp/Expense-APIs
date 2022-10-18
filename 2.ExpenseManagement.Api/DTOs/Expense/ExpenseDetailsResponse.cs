using System;
namespace _2.ExpenseManagement.Api.DTOs.Expense
{
    public class ExpenseDetailsResponse
    {
        public Guid ID { get; set; }

        public Guid TypeID { get; set; }

        public string? Type { get; set; }

        public DateTime? Date { get; set; }

        public decimal Cost { get; set; }

        public string? Description { get; set; }

        public Guid CategoryID { get; set; }

        public string? Category { get; set; }

        public IEnumerable<string?>? Attachments { get; set; }
    }
}

