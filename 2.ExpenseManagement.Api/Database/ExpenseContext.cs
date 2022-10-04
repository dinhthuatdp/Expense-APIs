using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using _2.ExpenseManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2.ExpenseManagement.Api.Database
{
    public class ExpenseContext : DbContext
    {
        #region ---- Variables ----
        public DbSet<Category>? Categories { get; set; }
        #endregion

        #region ---- Constructors ----
        public ExpenseContext(DbContextOptions options)
            : base(options)
        {
        }
        #endregion

        #region ---- Other methods ----
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            // Entities config.
            modelBuilder.ApplyConfiguration(new CategoryConfig());
        }

        public class CategoryConfig : IEntityTypeConfiguration<Category>
        {
            public void Configure(EntityTypeBuilder<Category> builder)
            {
                builder.ToTable("Category")
                    .HasKey(x => x.ID);
                builder.Property(x => x.ID)
                    .HasDefaultValueSql("NEWID()");
                builder.Property(x => x.Name)
                    .IsRequired();
            }
        }
        #endregion
    }
}

