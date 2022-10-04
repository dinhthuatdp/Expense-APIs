using System;
using System.ComponentModel.DataAnnotations;

namespace _2.ExpenseManagement.Api.DTOs.Categories
{
    public class CategoryAddRequest
    {
        [Required]
        public string? Name { get; set; }
    }
}

