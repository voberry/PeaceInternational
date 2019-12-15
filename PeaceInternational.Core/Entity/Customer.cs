using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class Customer : BaseEntity
    {
        public int FiscalYearId { get; set; }
        public string FileCodeNo { get; set; }
        public string TourName { get; set; }
        public string Country { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string Agent { get; set; }
        public string AgentStaff { get; set; }
        public string GuideName { get; set; }

        public FiscalYear FiscalYear { get; set; }

        public ICollection<Invoice> Invoice { get; set; }
        public ICollection<ServiceVoucher> ServiceVoucher { get; set; }
    }
}
