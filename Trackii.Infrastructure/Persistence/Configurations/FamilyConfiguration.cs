using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackii.Domain.Entities;

namespace Trackii.Infrastructure.Persistence.Configurations;

public sealed class FamilyConfiguration : IEntityTypeConfiguration<Family>
{
    public void Configure(EntityTypeBuilder<Family> builder)
    {
        builder.ToTable("family");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(f => f.AreaId)
            .HasColumnName("id_area")
            .IsRequired();

        builder.Property(f => f.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.Active)
            .HasColumnName("active")
            .IsRequired();

        // FK → Area
        builder.HasOne<Area>()
            .WithMany()
            .HasForeignKey(f => f.AreaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Regla: nombre único POR area (no global)
        builder.HasIndex(f => new { f.AreaId, f.Name })
            .IsUnique();
    }
}
