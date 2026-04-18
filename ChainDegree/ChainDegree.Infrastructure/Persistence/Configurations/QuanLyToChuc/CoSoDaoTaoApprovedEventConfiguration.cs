using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyToChuc.Aggregates;
using ChainDegree.Domain.QuanLyToChuc.Events;
using Microsoft.EntityFrameworkCore;

namespace ChainDegree.Infrastructure.Persistence.Configurations.QuanLyToChuc;

public class CoSoDaoTaoApprovedEventConfiguration : IEntityTypeConfiguration<CoSoDaoTaoApprovedEvent>
{
    [Obsolete]
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CoSoDaoTaoApprovedEvent> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EventType).IsRequired();

        builder.Property(e => e.Payload).IsRequired();

        builder.Property(e => e.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.IsProcessed)
                .HasDefaultValue(false);

        builder.Property(e => e.RetryCount)
                .HasDefaultValue(0);

        builder.HasIndex(e => new { e.IsProcessed, e.CreatedAt })
                .HasName("IX_UnprocessedEvents");
    }
}
