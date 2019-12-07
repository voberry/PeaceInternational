using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class ServiceVoucherConfig : BaseEntityConfig<ServiceVoucher>, IEntityTypeConfiguration<ServiceVoucher>
    {
        public override void Configure(EntityTypeBuilder<ServiceVoucher> builder)
        {
            base.Configure(builder);          

            builder.Property(p => p.ClientName)
              .HasMaxLength(255)
              .IsRequired();

            builder.Property(p => p.ArrivalDate)
               .HasColumnType("datetime");

            builder.Property(p => p.From)
               .IsRequired();

            builder.Property(p => p.ArrivalFlight)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.DepartureDate)
               .HasColumnType("datetime");

            builder.Property(p => p.To)
               .IsRequired();

            builder.Property(p => p.DepartureFlight)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Services);             

            builder.HasOne(p => p.Hotel)
                .WithMany(h => h.HotelReceipt)
                .HasForeignKey(p => p.HotelId);

            builder.HasOne(p => p.Customer)
                .WithMany(e => e.HotelReceipt)
                .HasForeignKey(p => p.FileCodeNo);
        }
    }
}
