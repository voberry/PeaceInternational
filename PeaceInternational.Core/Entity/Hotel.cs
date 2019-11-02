using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class Hotel : BaseEntity, IBaseEntity
    {
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
    }
}
