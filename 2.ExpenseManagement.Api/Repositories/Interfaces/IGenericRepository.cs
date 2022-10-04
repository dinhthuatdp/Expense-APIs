using System;
using System.Linq.Expressions;

namespace _2.ExpenseManagement.Api.Repositories.Interfaces
{
    /// <summary>
    /// Interface generic repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Find.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Get all.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> GetById(object id);

        /// <summary>
        /// Insert.
        /// </summary>
        /// <param name="obj"></param>
        void Insert(T obj);

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="obj"></param>
        void Update(T obj);

        /// <summary>
        /// Delete.
        /// </summary>
        /// <param name="id"></param>
        void Delete(T id);
    }
}

