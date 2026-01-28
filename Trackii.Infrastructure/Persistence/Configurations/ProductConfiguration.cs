using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackii.Domain.Entities;

namespace Trackii.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.SubfamilyId)
            .HasColumnName("id_subfamily")
            .IsRequired();

        builder.Property(p => p.PartNumber)
            .HasColumnName("part_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Active)
            .HasColumnName("active")
            .IsRequired();

        // FK → Subfamily
        builder.HasOne<Subfamily>()
            .WithMany()
            .HasForeignKey(p => p.SubfamilyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Regla: Part Number único global
        builder.HasIndex(p => p.PartNumber)
            .IsUnique();
    }
}
