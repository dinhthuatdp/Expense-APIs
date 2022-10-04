using System;
using System.Linq.Expressions;

namespace _2.ExpenseManagement.Api.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        IQueryable<T> GetAll();

        Task<T?> GetById(object id);

        void Insert(T obj);

        void Update(T obj);

        void Delete(T id);
    }
}

