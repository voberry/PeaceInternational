using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PeaceInternational.Web.Controllers
{
    [Authorize(Roles ="ADMIN")]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}