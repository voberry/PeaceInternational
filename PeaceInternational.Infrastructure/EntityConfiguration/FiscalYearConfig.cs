using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class FiscalYearConfig : BaseEntityConfig<FiscalYear>, IEntityTypeConfiguration<FiscalYear>
    {
        public override void Configure(EntityTypeBuilder<FiscalYear> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(p => p.EndDateBS)
                .IsRequired();

            builder.Property(p => p.StartDateBS)
                .IsRequired();

            builder.Property(p => p.StartDateAD)
                .HasColumnType("date");           

            builder.Property(p => p.EndDateAD)
                .HasColumnType("date");           

            builder.HasData(new FiscalYear() {
               Id = 1,
               Name = "76/77",
               StartDateBS = "2076/04/01",
               EndDateBS = "2077/03/31",
               StartDateAD = new DateTime(2019,07,17),
               EndDateAD = new DateTime(2020,07,15),
               CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 2,
                Name = "77/78",
                StartDateBS = "2077/04/01",
                EndDateBS = "2078/03/31",
                StartDateAD = new DateTime(2020,07,16),
                EndDateAD = new DateTime(2021,07,15),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 3,
                Name = "78/79",
                StartDateBS = "2078/04/01",
                EndDateBS = "2079/03/32",
                StartDateAD = new DateTime(2021,07,16),
                EndDateAD = new DateTime(2022,07,16),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 4,
                Name = "79/80",
                StartDateBS = "2079/04/01",
                EndDateBS = "2080/03/31",
                StartDateAD = new DateTime(2022,07,17),
                EndDateAD = new DateTime(2023,07,16),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 5,
                Name = "80/81",
                StartDateBS = "2080/04/01",
                EndDateBS = "2081/03/32",
                StartDateAD = new DateTime(2023,07,17),
                EndDateAD = new DateTime(2024,07,15),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 6,
                Name = "81/82",
                StartDateBS = "2081/04/01",
                EndDateBS = "2082/03/31",
                StartDateAD = new DateTime(2024,07,16),
                EndDateAD = new DateTime(2025,07,15),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 7,
                Name = "82/83",
                StartDateBS = "2082/04/01",
                EndDateBS = "2083/03/32",
                StartDateAD = new DateTime(2025,07,16),
                EndDateAD = new DateTime(2026,07,16),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 8,
                Name = "83/84",
                StartDateBS = "2083/04/01",
                EndDateBS = "2084/03/32",
                StartDateAD = new DateTime(2026,07,17),
                EndDateAD = new DateTime(2027,07,16),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 9,
                Name = "84/85",
                StartDateBS = "2084/04/01",
                EndDateBS = "2085/03/31",
                StartDateAD = new DateTime(2027,07,17),
                EndDateAD = new DateTime(2028,07,15),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 10,
                Name = "85/86",
                StartDateBS = "2085/04/01",
                EndDateBS = "2086/03/31",
                StartDateAD = new DateTime(2028,07,16),
                EndDateAD = new DateTime(2029,07,15),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 11,
                Name = "86/87",
                StartDateBS = "2086/04/01",
                EndDateBS = "2087/03/32",
                StartDateAD = new DateTime(2029,07,16),
                EndDateAD = new DateTime(2030,07,16),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 12,
                Name = "87/88",
                StartDateBS = "2087/04/01",
                EndDateBS = "2088/03/32",
                StartDateAD = new DateTime(2030,07,17),
                EndDateAD = new DateTime(2031,07,16),
                CreatedBy = "superadmin"
            });

            builder.HasData(new FiscalYear()
            {
                Id = 13,
                Name = "88/89",
                StartDateBS = "2088/04/01",
                EndDateBS = "2089/03/31",
                StartDateAD = new DateTime(2031,07,17),
                EndDateAD = new DateTime(2032,07,15),
                CreatedBy = "superadmin"
            });

        }
    }
}
