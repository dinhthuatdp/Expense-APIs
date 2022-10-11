using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using _2.ExpenseManagement.Api.Database.Configurations;
using _2.ExpenseManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2.ExpenseManagement.Api.Database
{
    /// <summary>
    /// Expense context.
    /// </summary>
    public class ExpenseContext : DbContext
    {
        #region ---- Variables ----
        public DbSet<Category>? Categories { get; set; }
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public ExpenseContext(DbContextOptions options)
            : base(options)
        {
        }
        #endregion

        #region ---- Other methods ----
        /// <summary>
        /// On model creating.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            // Entities config.
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new EntityTypeConfig());
        }
        #endregion
    }
}

