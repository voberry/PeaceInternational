using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class Invoice : BaseEntity, IBaseEntity
    {
        public Invoice()
        {
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
        }
       
        public string InvoiceNo { get; set; }
        public int FileCodeNo { get; set; }
        public string RefrenceNo { get; set; }
        public string Dr { get; set; }
        public string AgentName { get; set; }
        public string ClientName { get; set; }
        public string Currency { get; set; }
        public int PAX { get; set; }

        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
