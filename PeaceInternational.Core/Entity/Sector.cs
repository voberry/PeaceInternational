using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class Sector : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal FullDayRate { get; set; }
        public decimal HalfDayRate { get; set; }
    }
}
