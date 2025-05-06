using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class BranchMapping : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(b => b.BranchId);

        builder.Property(b => b.BranchName)
            .HasMaxLength(200);

        builder.Property(b => b.Email)
            .HasMaxLength(100);

        builder.Property(b => b.Phone)
            .HasMaxLength(20);

        builder.Property(b => b.IsBranch)
            .HasDefaultValue(true);

        builder.HasOne<Customer>()
            .WithMany(c => c.Branches)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 