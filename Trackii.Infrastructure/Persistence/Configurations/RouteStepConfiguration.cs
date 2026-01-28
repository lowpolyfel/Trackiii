using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackii.Domain.Entities;

namespace Trackii.Infrastructure.Persistence.Configurations;

public sealed class RouteStepConfiguration : IEntityTypeConfiguration<RouteStep>
{
    public void Configure(EntityTypeBuilder<RouteStep> builder)
    {
        builder.ToTable("route_step");

        builder.HasKey(rs => rs.Id);

        builder.Property(rs => rs.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(rs => rs.RouteId)
            .HasColumnName("route_id")
            .IsRequired();

        builder.Property(rs => rs.StepNumber)
            .HasColumnName("step_number")
            .IsRequired();

        builder.Property(rs => rs.LocationId)
            .HasColumnName("location_id")
            .IsRequired();

        // FK → Route
        builder.HasOne<Route>()
            .WithMany()
            .HasForeignKey(rs => rs.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        // FK → Location
        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(rs => rs.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Regla estructural: no hay pasos paralelos ni duplicados
        builder.HasIndex(rs => new { rs.RouteId, rs.StepNumber })
            .IsUnique();

        // Índices útiles para búsquedas
        builder.HasIndex(rs => rs.RouteId);
        builder.HasIndex(rs => rs.LocationId);
    }
}
