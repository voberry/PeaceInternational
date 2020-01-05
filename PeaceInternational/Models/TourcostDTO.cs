using PeaceInternational.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeaceInternational.Web.Models
{
    public class TourcostDTO
    {        
        public Tourcost Tourcost { get; set; }
        public IList<TourcostDetail> TourcostDetail { get; set; }
    }
}
