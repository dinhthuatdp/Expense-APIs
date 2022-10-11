using System;
using _2.ExpenseManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2.ExpenseManagement.Api.Database.Configurations
{
    /// <summary>
    /// Entity Type config
    /// </summary>
    public class EntityTypeConfig : IEntityTypeConfiguration<EntityType>
    {
        public void Configure(EntityTypeBuilder<EntityType> builder)
        {
            builder.ToTable("EntityType")
                .HasKey(x => x.ID);
            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWID()");
            builder.HasIndex(x => new { x.Name, x.Type })
                .IsUnique();
        }
    }
}

