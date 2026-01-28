using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackii.Domain.Entities;

namespace Trackii.Infrastructure.Persistence.Configurations;

public sealed class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.ToTable("route");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(r => r.SubfamilyId)
            .HasColumnName("subfamily_id")
            .IsRequired();

        builder.Property(r => r.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.Version)
            .HasColumnName("version")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(r => r.Active)
            .HasColumnName("active")
            .IsRequired();

        // FK → Subfamily
        builder.HasOne<Subfamily>()
            .WithMany()
            .HasForeignKey(r => r.SubfamilyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índice para búsquedas frecuentes por subfamily
        builder.HasIndex(r => r.SubfamilyId);

        // (Opcional, recomendado)
        // Evita duplicados exactos de nombre+versión dentro de una subfamily
        builder.HasIndex(r => new { r.SubfamilyId, r.Name, r.Version })
            .IsUnique();
    }
}
