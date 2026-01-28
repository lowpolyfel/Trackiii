using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Ruta: Trackii.Infrastructure/Persistence/Configurations/SubfamilyConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackii.Domain.Entities;

namespace Trackii.Infrastructure.Persistence.Configurations;

public sealed class SubfamilyConfiguration : IEntityTypeConfiguration<Subfamily>
{
    public void Configure(EntityTypeBuilder<Subfamily> builder)
    {
        builder.ToTable("subfamily");

        builder.HasKey(sf => sf.Id);

        builder.Property(sf => sf.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(sf => sf.FamilyId)
            .HasColumnName("id_family")
            .IsRequired();

        builder.Property(sf => sf.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(sf => sf.Active)
            .HasColumnName("active")
            .IsRequired();

        // FK → Family
        builder.HasOne<Family>()
            .WithMany()
            .HasForeignKey(sf => sf.FamilyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Regla: nombre único POR family
        builder.HasIndex(sf => new { sf.FamilyId, sf.Name })
            .IsUnique();
    }
}
