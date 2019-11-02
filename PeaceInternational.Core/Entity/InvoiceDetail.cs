using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class InvoiceDetail: BaseEntity, IBaseEntity
    {
        public int InvoiceId { get; set; }
        public string Particulars { get; set; }
        public double Amount { get; set; }

        public Invoice Invoice { get; set; }
    }
}
