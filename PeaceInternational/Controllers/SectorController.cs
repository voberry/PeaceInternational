using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeaceInternational.Core.Entity;
using PeaceInternational.Core.IRepository;
using PeaceInternational.Web.Models;

namespace PeaceInternational.Web.Controllers
{
    [Authorize]
    public class SectorController : Controller
    {
        private readonly ICrudService<Sector> _sectorCrudService;
        private readonly UserManager<IdentityUser> _userManager;
        private Notification notification;

        public SectorController(
            ICrudService<Sector> sectorCrudService,
            UserManager<IdentityUser> userManager)
        {
            _sectorCrudService = sectorCrudService;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //GET Sector
        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                if (id == null)
                {
                    var result = await _sectorCrudService.GetAllAsync();
                    return Json(result);
                }
                else
                {
                    var result = _sectorCrudService.Get(id);
                    return Json(result);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        //Save Sector
        [HttpPost]
        public async Task<IActionResult> Save(Sector sector)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                notification = new Notification();

                if (sector.Id > 0)
                {

                    notification = await EditSector(sector, user);
                }
                else
                {
                    await _sectorCrudService.InsertAsync(new Sector
                    {
                        Name = sector.Name,
                        Code = sector.Code,
                        FullDayRate = sector.FullDayRate,
                        HalfDayRate = sector.HalfDayRate,
                        CreatedBy = user.Id
                    });

                    notification.Type = "success";
                    notification.Message = "Sector created successfully.";
                }

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Sector creation failed.";
                return Json(notification);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                notification = new Notification();
                notification = DeleteSector(id);

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Sector deletion failed.";
                return Json(notification);
            }
        }

        private async Task<Notification> EditSector(Sector sector, IdentityUser user)
        {
            try
            {
                var record = await _sectorCrudService.GetAsync(sector.Id);

                record.Name = sector.Name;
                record.Code = sector.Code;
                record.FullDayRate = sector.FullDayRate;
                record.HalfDayRate = sector.HalfDayRate;
                record.ModifiedBy = user.Id;
                record.ModifiedDate = DateTime.Now;

                _sectorCrudService.Update(record);

                return new Notification("success", "Sector updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Sector update failed.");
            }
        }

        private Notification DeleteSector(int id)
        {
            try
            {
                var record = _sectorCrudService.Get(id);
                _sectorCrudService.Delete(record);

                return new Notification("success", "Sector deleted successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Failed to delete sector.");
            }
        }
    }
}