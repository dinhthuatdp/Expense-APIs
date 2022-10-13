using System;
namespace _2.ExpenseManagement.Api.Entities
{
    public class EntityType : BaseEntity
    {
        public Guid ID { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public virtual ICollection<Expense>? Expenses { get; set; }
    }
}

