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

public sealed class WipItemConfiguration : IEntityTypeConfiguration<WipItem>
{
    public void Configure(EntityTypeBuilder<WipItem> builder)
    {
        builder.ToTable("wip_item");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(w => w.WorkOrderId)
            .HasColumnName("wo_order_id")
            .IsRequired();

        builder.Property(w => w.CurrentStepId)
            .HasColumnName("current_step_id")
            .IsRequired();

        builder.Property(w => w.Status)
            .HasColumnName("status")
            .HasConversion<string>() // enum → varchar
            .IsRequired();

        builder.Property(w => w.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(w => w.QtyInput)
            .HasColumnName("qty_input")
            .IsRequired();

        // FK → WorkOrder
        builder.HasOne<WorkOrder>()
            .WithMany()
            .HasForeignKey(w => w.WorkOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // FK → RouteStep (step actual)
        builder.HasOne<RouteStep>()
            .WithMany()
            .HasForeignKey(w => w.CurrentStepId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices útiles
        builder.HasIndex(w => w.WorkOrderId);
        builder.HasIndex(w => w.Status);
        builder.HasIndex(w => w.CurrentStepId);
    }
}
