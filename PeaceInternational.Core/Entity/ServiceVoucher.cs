using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class ServiceVoucher : BaseEntity
    {
        public string ExchangeOrderNo { get; set; }
        public string FileCodeNo { get; set; }
        public int FiscalYearId { get; set; }
        public int HotelId { get; set; }
        public string ClientName { get; set; }
        public int PAX { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string From { get; set; }
        public string ArrivalFlight { get; set; }
        public DateTime DepartureDate { get; set; }
        public string To { get; set; }
        public string DepartureFlight { get; set; }
        public string Services { get; set; }

        public Customer Customer { get; set; }
        public FiscalYear FiscalYear { get; set; }

        public Hotel Hotel { get; set; }
    }
}
