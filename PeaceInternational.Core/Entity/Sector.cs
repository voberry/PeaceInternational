using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core.Entity
{
    public class Sector : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public ICollection<SectorTransport> SectorTransport { get; set; }

        public ICollection<TourcostDetail> TourcostDetail1 { get; set; }
        public ICollection<TourcostDetail> TourcostDetail2 { get; set; }
    }
}
