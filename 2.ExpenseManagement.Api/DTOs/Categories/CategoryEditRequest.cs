using System;
using System.ComponentModel.DataAnnotations;

namespace _2.ExpenseManagement.Api.DTOs.Categories
{
    public class CategoryEditRequest
    {
        [Required]
        public string? Name { get; set; }
    }
}

