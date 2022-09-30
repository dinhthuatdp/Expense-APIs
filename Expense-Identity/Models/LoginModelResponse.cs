namespace Expense_Identity.Models
{
    public class LoginModelResponse
    {
        public string? Token { get; set; }

        public DateTime? Expiration { get; set; }
    }
}

