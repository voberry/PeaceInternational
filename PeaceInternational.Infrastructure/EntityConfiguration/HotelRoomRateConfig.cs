using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class HotelRoomRateConfig : BaseEntityConfig<HotelRoomRate>, IEntityTypeConfiguration<HotelRoomRate>
    {
        public override void Configure(EntityTypeBuilder<HotelRoomRate> builder)
        {
            base.Configure(builder);

            builder.HasIndex(p => p.HotelId)
                .IsUnique();

            builder.Property(p => p.SingleBed)
                .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.DoubleBed)
                .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.ExtraBed)
                .HasColumnType("Decimal(10,2)");           

            builder.Property(p => p.AP)
               .HasColumnType("Decimal(10,2)");

            builder.Property(p => p.MAP)
               .HasColumnType("Decimal(10,2)");

            builder.HasOne(p => p.Hotel)
                .WithOne(e => e.HotelRoomRate)
                .HasForeignKey<HotelRoomRate>(p => p.HotelId);
        }
    }
}
