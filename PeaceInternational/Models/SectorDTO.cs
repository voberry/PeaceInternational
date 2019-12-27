using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeaceInternational.Web.Models
{
    public class SectorDTO
    {
        public Sector Sector { get; set; }
        public SectorTransport[] SectorTransport { get; set; }

    }
}
