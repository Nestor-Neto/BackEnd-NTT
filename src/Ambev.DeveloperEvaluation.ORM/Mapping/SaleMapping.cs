using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleMapping : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("sales");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(s => s.Date)
                .HasColumnName("sale_date")
                .IsRequired();

            builder.Property(s => s.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();

            builder.Property(s => s.CustomerName)
                .HasColumnName("customer_name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(s => s.BranchId)
                .HasColumnName("branch_id")
                .IsRequired();

            builder.Property(s => s.BranchName)
                .HasColumnName("branch_name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(s => s.Cancelled)
                .HasColumnName("cancelled")
                .IsRequired();

            builder.HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey("sale_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
