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
        public bool IsTicket { get; set; }
        public string FileCodeNo { get; set; }
        public int FiscalYearId { get; set; }
        public string ReferenceNo { get; set; }
        public string Dr { get; set; }
        public string Address { get; set; }
        public string ClientName { get; set; }
        public string Currency { get; set; }
        public int PAX { get; set; }
        public string Guide { get; set; }
        public string Vehicle { get; set; }
        public decimal TotalDue { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }

        public Customer Customer { get; set; }
        public FiscalYear FiscalYear { get; set; }

        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        
    }
}
