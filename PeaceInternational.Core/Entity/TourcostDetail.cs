using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class TourcostDetail : BaseEntity
    {
        public string Day { get; set; }
        public int TourcostId { get; set; }
        public int Sector1Id { get; set; }
        public int? Sector2Id { get; set; }
        public int? Sector3Id { get; set; }
        public int? HotelAId { get; set; }
        public int? HotelBId { get; set; }
        public int? HotelCId { get; set; }
        public decimal? Category1Cost { get; set; }
        public decimal? Category2Cost { get; set; }
        public decimal? Category3Cost { get; set; }
        public decimal? Category4Cost { get; set; }
        public decimal? Category5Cost { get; set; }

        public Tourcost Tourcost { get; set; }

        public Sector Sector1 { get; set; }
        public Sector Sector2 { get; set; }
        public Sector Sector3 { get; set; }

        public Hotel HotelA { get; set; }
        public Hotel HotelB { get; set; }
        public Hotel HotelC { get; set; }
    }
}
