using System;
namespace _2.ExpenseManagement.Api.Entities
{
    public class Category : BaseEntity
    {
        public Guid ID { get; set; }

        public string? Name { get; set; }
    }
}

