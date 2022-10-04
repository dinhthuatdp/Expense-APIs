using System;
using _2.ExpenseManagement.Api.Entities;
using _2.ExpenseManagement.Api.Repositories.Interfaces;

namespace _2.ExpenseManagement.Api.UoW
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();

        IGenericRepository<Category> CategoryRepository { get; }
    }
}

