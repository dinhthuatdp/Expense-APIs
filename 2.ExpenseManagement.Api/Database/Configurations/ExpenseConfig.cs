using System;
using _2.ExpenseManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2.ExpenseManagement.Api.Database.Configurations
{
    public class ExpenseConfig : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expense")
                .HasKey(x => x.ID);
            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWID()");
            builder.HasOne<EntityType>(x => x.Type)
                .WithMany(x => x.Expenses)
                .HasForeignKey(x => x.TypeID);
            builder.HasOne<Category>(x => x.Category)
                .WithMany(x => x.Expenses)
                .HasForeignKey(x => x.CategoryID);
        }
    }
}

