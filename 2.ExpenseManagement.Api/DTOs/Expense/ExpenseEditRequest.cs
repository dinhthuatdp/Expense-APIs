using System;
using System.ComponentModel.DataAnnotations;

namespace _2.ExpenseManagement.Api.DTOs.Expense
{
    public class ExpenseEditRequest
    {
        [Required]
        public Guid TypeID { get; set; }

        [Required]
        public Guid CategoryID { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        public decimal Cost { get; set; }

        public string? Description { get; set; }

        public List<IFormFile>? Attachments { get; set; }
    }
}

