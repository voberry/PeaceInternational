using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class SectorConfig : BaseEntityConfig<Sector> , IEntityTypeConfiguration<Sector>
    {
        public override void Configure(EntityTypeBuilder<Sector> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(10);            

            builder.HasMany(p => p.TourcostDetail1)
                .WithOne(e => e.Sector1)
                .HasForeignKey(e => e.Sector1Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(p => p.TourcostDetail2)
             .WithOne(e => e.Sector2)
             .HasForeignKey(e => e.Sector2Id)
             .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
