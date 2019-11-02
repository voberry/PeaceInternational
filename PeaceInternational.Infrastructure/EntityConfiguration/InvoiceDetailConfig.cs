using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeaceInternational.Core.Entity;

namespace PeaceInternational.Infrastructure.EntityConfiguration
{
    public class InvoiceDetailConfig: BaseEntityConfig<InvoiceDetail>, IEntityTypeConfiguration<InvoiceDetail>
    {
        public override void Configure(EntityTypeBuilder<InvoiceDetail> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Particulars)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Amount)
                .HasColumnType("float");
        }
    }
}
