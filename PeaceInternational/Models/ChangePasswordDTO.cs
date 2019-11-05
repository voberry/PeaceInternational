using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeaceInternational.Web.Models
{
    public class ChangePasswordDTO
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
