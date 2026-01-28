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

public sealed class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.ToTable("work_order");

        builder.HasKey(wo => wo.Id);

        builder.Property(wo => wo.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(wo => wo.WoNumber)
            .HasColumnName("wo_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(wo => wo.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(wo => wo.Status)
            .HasColumnName("status")
            .HasConversion<string>() // enum → varchar
            .IsRequired();

        // FK → Product
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(wo => wo.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Regla: WO Number único
        builder.HasIndex(wo => wo.WoNumber)
            .IsUnique();

        // Índice útil por status
        builder.HasIndex(wo => wo.Status);
    }
}
