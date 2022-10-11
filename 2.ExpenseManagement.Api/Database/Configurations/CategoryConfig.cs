using System;
using _2.ExpenseManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2.ExpenseManagement.Api.Database.Configurations
{
    /// <summary>
    /// Category config.
    /// </summary>
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
}

