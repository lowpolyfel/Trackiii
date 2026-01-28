using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackii.Domain.Entities;
using Trackii.Domain.Enums;

namespace Trackii.Infrastructure.Persistence.Configurations;

public sealed class ScanEventConfiguration : IEntityTypeConfiguration<ScanEvent>
{
    public void Configure(EntityTypeBuilder<ScanEvent> builder)
    {
        builder.ToTable("scan_event");

        builder.HasKey(se => se.Id);

        builder.Property(se => se.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(se => se.WipItemId)
            .HasColumnName("wip_item_id")
            .IsRequired();

        builder.Property(se => se.RouteStepId)
            .HasColumnName("route_step_id")
            .IsRequired();

        builder.Property(se => se.ScanType)
            .HasColumnName("scan_type")
            .HasConversion<string>() // enum → varchar
            .IsRequired();

        builder.Property(se => se.Timestamp)
            .HasColumnName("ts")
            .IsRequired();

        // FK → WipItem
        builder.HasOne<WipItem>()
            .WithMany()
            .HasForeignKey(se => se.WipItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // FK → RouteStep
        builder.HasOne<RouteStep>()
            .WithMany()
            .HasForeignKey(se => se.RouteStepId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices útiles para auditoría
        builder.HasIndex(se => se.WipItemId);
        builder.HasIndex(se => se.RouteStepId);
        builder.HasIndex(se => se.Timestamp);
    }
}
