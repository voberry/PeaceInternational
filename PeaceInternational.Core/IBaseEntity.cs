using System;
using System.Collections.Generic;
using System.Text;

namespace PeaceInternational.Core
{
    interface IBaseEntity
    {
        int Id { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        string ModifiedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}
