using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class HotelRoomRate : BaseEntity
    {
        public int HotelId { get; set; }
        public decimal SingleBed { get; set; }
        public decimal DoubleBed { get; set; }
        public decimal ExtraBed { get; set; }

        public Hotel Hotel { get; set; }
    }
}
