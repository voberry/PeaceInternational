using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .UseSqlServerIdentityColumn();

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.ModifiedBy)
               .HasMaxLength(256);

            builder.Property(p => p.ModifiedDate)
                .HasColumnType("datetime");



        }        
    }
}
