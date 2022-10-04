using System;
using _2.ExpenseManagement.Api.Database;
using _2.ExpenseManagement.Api.Entities;
using _2.ExpenseManagement.Api.Repositories;
using _2.ExpenseManagement.Api.Repositories.Interfaces;

namespace _2.ExpenseManagement.Api.UoW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ExpenseContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed = false;
        private IGenericRepository<Category>? categoryRepository;

        public UnitOfWork(ExpenseContext context,
            ILogger<UnitOfWork> logger)
        {
            _logger = logger;
            _context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed &&
                disposing)
            {
                _context.Dispose();
            }
            GC.SuppressFinalize(this);

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangeAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository is null)
                {
                    categoryRepository = new GenericRepository<Category>(_context, _logger);
                }
                return categoryRepository;
            }
        }
    }
}

