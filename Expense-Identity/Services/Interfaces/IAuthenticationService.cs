using Expense_Identity.Models;

namespace Expense_Identity.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response<LoginModelResponse>> Login(LoginModelRequest model);

        Task<Response<RegisterModelResponse>> Register(RegisterModelRequest model);

        Task<Response<RegisterModelResponse>> RegisterAdmin(RegisterModelRequest model);
    }
}

