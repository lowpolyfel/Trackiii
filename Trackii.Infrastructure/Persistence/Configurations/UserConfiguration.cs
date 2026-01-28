using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackii.Domain.Entities;

namespace Trackii.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.RoleId)
            .HasColumnName("role_id")
            .IsRequired();

        builder.Property(u => u.Active)
            .HasColumnName("active")
            .IsRequired();

        // FK → Role
        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Regla: username único
        builder.HasIndex(u => u.Username)
            .IsUnique();

        // Índice útil por rol
        builder.HasIndex(u => u.RoleId);
    }
}