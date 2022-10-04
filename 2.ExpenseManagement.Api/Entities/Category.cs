using System;
namespace _2.ExpenseManagement.Api.Entities
{
    public class Category : BaseTracking
    {
        public Guid ID { get; set; }

        public string? Name { get; set; }
    }
}

