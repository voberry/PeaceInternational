using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeaceInternational.Infrastructure.EntityConfiguration;

namespace PeaceInternational.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new HotelConfig());
            builder.ApplyConfiguration(new InvoiceConfig());
            builder.ApplyConfiguration(new InvoiceDetailConfig());
            builder.ApplyConfiguration(new ServiceVoucherConfig());
            builder.ApplyConfiguration(new CustomerConfig());
            builder.ApplyConfiguration(new FiscalYearConfig());
            builder.ApplyConfiguration(new GuideEntityConfig());
            builder.ApplyConfiguration(new HotelRoomRateConfig());
            builder.ApplyConfiguration(new SectorConfig());
            builder.ApplyConfiguration(new TransportConfig());
            builder.ApplyConfiguration(new TourcostConfig());
            builder.ApplyConfiguration(new TourcostDetailConfig());
            builder.ApplyConfiguration(new SectorTransportConfig());
        }
    }
}
