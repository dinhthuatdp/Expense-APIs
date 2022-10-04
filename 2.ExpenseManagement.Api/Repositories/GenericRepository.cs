﻿using System;
using System.Linq.Expressions;
using _2.ExpenseManagement.Api.Database;
using _2.ExpenseManagement.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _2.ExpenseManagement.Api.Repositories
{
    /// <summary>
    /// Generic repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region ---- Variables ----
        private readonly ExpenseContext _expenseContext;
        private readonly ILogger _logger;
        private readonly DbSet<T> _dbSet;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expenseContext"></param>
        /// <param name="logger"></param>
        public GenericRepository(ExpenseContext expenseContext,
            ILogger logger)
        {
            _expenseContext = expenseContext;
            _logger = logger;
            _dbSet = _expenseContext.Set<T>();
        }
        #endregion

        #region ---- Public methods ----
        /// <summary>
        /// Delete.
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            var result = _expenseContext.Set<T>()
                .Remove(entity);
        }

        /// <summary>
        /// Find.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            var result = _expenseContext.Set<T>()
                .Where(expression);

            return result;
        }

        /// <summary>
        /// Get all.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            var result = _expenseContext.Set<T>();

            return result;
        }

        /// <summary>
        /// Get by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> GetById(object id)
        {
            var result = await _expenseContext.Set<T>()
                .FindAsync(id);
            return result;
        }

        /// <summary>
        /// Insert new.
        /// </summary>
        /// <param name="obj"></param>
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

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="obj"></param>
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
        #endregion
    }
}
