using System.ComponentModel.DataAnnotations;

namespace Expense_Identity.Models
{
    public class LoginModelRequest
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}

