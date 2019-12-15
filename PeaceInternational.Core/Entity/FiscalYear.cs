using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class FiscalYear: BaseEntity
    {
        public string Name { get; set; }
        public string StartDateBS { get; set; }
        public string EndDateBS { get; set; }
        public DateTime StartDateAD { get; set; }
        public DateTime EndDateAD { get; set; }

        public ICollection<Customer> Customers { get; set; }
        public ICollection<Invoice> Invoice { get; set; }
        public ICollection<ServiceVoucher> ServiceVoucher { get; set; }
    }
}
