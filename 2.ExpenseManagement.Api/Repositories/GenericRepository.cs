using System;
using System.Linq.Expressions;
using _2.ExpenseManagement.Api.Database;
using _2.ExpenseManagement.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _2.ExpenseManagement.Api.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ExpenseContext _expenseContext;
        private readonly ILogger _logger;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ExpenseContext expenseContext,
            ILogger logger)
        {
            _expenseContext = expenseContext;
            _logger = logger;
            _dbSet = _expenseContext.Set<T>();
        }

        public void Delete(T entity)
        {
            var result = _expenseContext.Set<T>()
                .Remove(entity);
        }


        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            var result = _expenseContext.Set<T>()
                .Where(expression);

            return result;
        }

        public IQueryable<T> GetAll()
        {
            var result = _expenseContext.Set<T>();

            return result;
        }

        public async Task<T?> GetById(object id)
        {
            var result = await _expenseContext.Set<T>()
                .FindAsync(id);
            return result;
        }

        public void Insert(T obj)
        {
            if (obj is null)
            {
                _logger.LogInformation($"{this.GetType().Name} Insert null entity");
                return;
            }
            _expenseContext.Set<T>()
                .Add(obj);
        }

        public void Update(T obj)
        {
            if (obj is null)
            {
                _logger.LogInformation($"{this.GetType().Name} Update null entity");
                return;
            }
            _expenseContext.Set<T>()
                 .Update(obj);
        }
    }
}

