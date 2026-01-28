using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackii.Domain.Entities;

namespace Trackii.Infrastructure.Persistence.Configurations;

public sealed class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("devices");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(d => d.DeviceUid)
            .HasColumnName("device_uid")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.LocationId)
            .HasColumnName("location_id")
            .IsRequired();

        builder.Property(d => d.Name)
            .HasColumnName("name")
            .HasMaxLength(100);

        builder.Property(d => d.Active)
            .HasColumnName("active")
            .IsRequired();

        // FK → Location
        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(d => d.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Regla técnica: device_uid único
        builder.HasIndex(d => d.DeviceUid)
            .IsUnique();

        // Índice útil para búsquedas por location
        builder.HasIndex(d => d.LocationId);
    }
}

