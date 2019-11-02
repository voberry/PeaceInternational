using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core
{
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
