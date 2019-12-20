using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class Transport : BaseEntity
    {
        public string Name { get; set; }
        public int MinPAX { get; set; }
        public int MaxPAX { get; set; }
        public double FullDayRate { get; set; }
        public double HalfDayRate { get; set; }
    }
}
