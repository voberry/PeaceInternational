using Newtonsoft.Json;
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

        [JsonIgnore]
        public ICollection<TourcostDetail> TourcostDetail1 { get; set; }
        [JsonIgnore]
        public ICollection<TourcostDetail> TourcostDetail2 { get; set; }
        [JsonIgnore]
        public ICollection<TourcostDetail> TourcostDetail3 { get; set; }
    }
}
