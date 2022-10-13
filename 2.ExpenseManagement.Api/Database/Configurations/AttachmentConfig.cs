using System;
using _2.ExpenseManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2.ExpenseManagement.Api.Database.Configurations
{
    /// <summary>
    /// Attachment config.
    /// </summary>
    public class AttachmentConfig : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.ToTable("Attachment")
                .HasKey(x => x.ID);
            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWID()");
            builder.Property(x => x.Url)
                .IsRequired();
            builder.HasOne<Expense>(x => x.Expense)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.ExpenseID);
        }
    }
}

