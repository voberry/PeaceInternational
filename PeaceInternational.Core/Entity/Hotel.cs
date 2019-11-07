using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class Hotel : BaseEntity, IBaseEntity
    {
        public Hotel()
        {
            this.HotelReceipt = new HashSet<HotelReceipt>();
        }

        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }

        public ICollection<HotelReceipt> HotelReceipt { get; set; }
    }
}
