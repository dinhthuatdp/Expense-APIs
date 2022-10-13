using System;
using _2.ExpenseManagement.Api.Database;
using _2.ExpenseManagement.Api.Entities;
using _2.ExpenseManagement.Api.Repositories;
using _2.ExpenseManagement.Api.Repositories.Interfaces;
using CommonLib.Middlewares;
using CommonLib.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _2.ExpenseManagement.Api.UoW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ExpenseContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed = false;
        private IGenericRepository<Category>? categoryRepository;
        private IGenericRepository<EntityType>? entityTypeRepository;
        private IGenericRepository<Attachment>? attachmentRepository;
        private IGenericRepository<Expense>? expenseRepository;

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
            UpdateTrackingField();
            var result = await _context.SaveChangesAsync();
            return result;
        }

        private void UpdateTrackingField()
        {
            var entries = _context.ChangeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity &&
                (x.State == EntityState.Added ||
                 x.State == EntityState.Modified));

            if (entries != null)
            {
                if (CurrentUser.User is null)
                {
                    _logger.LogError($"{this.GetType().Name} Error: CurrentUser is null");
                    return;
                }
                BaseEntity baseEntity;
                foreach (var entityEntry in entries)
                {
                    baseEntity = ((BaseEntity)entityEntry.Entity);
                    if (entityEntry.State == EntityState.Added)
                    {
                        baseEntity.CreatedBy = CurrentUser.User.UserName;
                        baseEntity.CreatedDate = DateTime.UtcNow;
                    }
                    else
                    {
                        baseEntity.UpdatedBy = CurrentUser.User.UserName;
                        baseEntity.UpdatedDate = DateTime.UtcNow;
                    }
                }
            }
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

        public IGenericRepository<EntityType> EntityTypeRepository
        {
            get
            {
                if (this.entityTypeRepository is null)
                {
                    entityTypeRepository = new GenericRepository<EntityType>(_context, _logger);
                }
                return entityTypeRepository;
            }
        }

        public IGenericRepository<Attachment> AttachmentRepository
        {
            get
            {
                if (this.attachmentRepository is null)
                {
                    attachmentRepository = new GenericRepository<Attachment>(_context, _logger);
                }

                return attachmentRepository;
            }
        }

        public IGenericRepository<Expense> ExpenseRepository
        {
            get
            {
                if (this.expenseRepository is null)
                {
                    expenseRepository = new GenericRepository<Expense>(_context, _logger);
                }

                return expenseRepository;
            }
        }
    }
}

