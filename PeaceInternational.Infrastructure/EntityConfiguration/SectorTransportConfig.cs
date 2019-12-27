using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class SectorTransportConfig : BaseEntityConfig<SectorTransport>, IEntityTypeConfiguration<SectorTransport>
    {
        public override void Configure(EntityTypeBuilder<SectorTransport> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.HalfDayCost)
              .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.FullDayCost)
             .HasColumnType("Decimal(10,2)");

            builder.HasOne(p => p.Sector)
                .WithMany(e => e.SectorTransport)
                .HasForeignKey(p => p.SectorId);

            builder.HasOne(p => p.Transport)
             .WithMany(e => e.SectorTransport)
             .HasForeignKey(p => p.TransportId);

        }
    }
}
