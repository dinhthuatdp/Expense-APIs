using System;
namespace _2.ExpenseManagement.Api.Entities
{
    public class Attachment : BaseEntity
    {
        public Guid ID { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public Guid ExpenseID { get; set; }

        public virtual Expense? Expense { get; set; }
    }
}

