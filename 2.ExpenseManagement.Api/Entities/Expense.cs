using System;
namespace _2.ExpenseManagement.Api.Entities
{
    public class Expense : BaseEntity
    {
        public Guid ID { get; set; }

        public DateTime? Date { get; set; }

        public decimal Cost { get; set; }

        public string? Description { get; set; }

        public Guid TypeID { get; set; }

        public Guid CategoryID { get; set; }

        public virtual EntityType? Type { get; private set; }

        public virtual Category? Category { get; private set; }

        public virtual ICollection<Attachment>? Attachments { get; set; }
    }
}

