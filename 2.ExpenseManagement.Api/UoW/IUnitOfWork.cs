using System;
using _2.ExpenseManagement.Api.Entities;
using _2.ExpenseManagement.Api.Repositories.Interfaces;

namespace _2.ExpenseManagement.Api.UoW
{
    /// <summary>
    /// Interface unit of work.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Save change async.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangeAsync();

        /// <summary>
        /// Category repository.
        /// </summary>
        IGenericRepository<Category> CategoryRepository { get; }

        /// <summary>
        /// Entity Type Repository.
        /// </summary>
        IGenericRepository<EntityType> EntityTypeRepository { get; }
    }
}

