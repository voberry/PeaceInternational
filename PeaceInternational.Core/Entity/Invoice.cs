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
        public string ReferenceNo { get; set; }
        public string Dr { get; set; }
        public string AgentName { get; set; }
        public string ClientName { get; set; }
        public string Currency { get; set; }
        public int PAX { get; set; }
        public string Guide { get; set; }
        public string Vehicle { get; set; }
        public double TotalDue { get; set; }
        public double Discount { get; set; }
        public double NetAmount { get; set; }

        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
