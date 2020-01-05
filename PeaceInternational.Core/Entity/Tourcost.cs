using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class Tourcost : BaseEntity
    {
        public string ClientName { get; set; }
        public int MinPAX { get; set; }
        public int MaxPAX { get; set; }
        public int Days { get; set; }
        public string Category1 { get; set; }       
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public string Category4 { get; set; }
        public string Category5 { get; set; }
        public int GuideId { get; set; }
        public bool IsMealAP { get; set; }
        public bool IsLuxury { get; set; }
        public int  GuideType { get; set; }        
        public int DiscountTransportation { get; set; }
        public int DiscountAccomodation { get; set; }        
        public string Comment { get; set; }

        public Guide Guide { get; set; }

        public ICollection<TourcostDetail> TourcostDetail { get; set; }
    }
}
