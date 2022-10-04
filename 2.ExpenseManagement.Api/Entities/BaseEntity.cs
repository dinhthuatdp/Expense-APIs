using System;
namespace _2.ExpenseManagement.Api.Entities
{
    /// <summary>
    /// Base Trangking Model.
    /// </summary>
    public abstract class BaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }
    }
}

