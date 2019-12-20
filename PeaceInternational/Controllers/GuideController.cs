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
    public class GuideController : Controller
    {
        private readonly ICrudService<Guide> _guideCrudService;
        private readonly UserManager<IdentityUser> _userManager;
        private Notification notification;

        public GuideController(
            ICrudService<Guide> guideCrudService,
            UserManager<IdentityUser> userManager)
        {
            _guideCrudService = guideCrudService;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //GET Guide
        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                if (id == null)
                {
                    var result = await _guideCrudService.GetAllAsync();
                    return Json(result);
                }
                else
                {
                    var result = _guideCrudService.Get(id);
                    return Json(result);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        //Save Guide
        [HttpPost]
        public async Task<IActionResult> Save(Guide guide)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                notification = new Notification();

                if (guide.Id > 0)
                {

                    notification = await EditGuide(guide, user);
                }
                else
                {
                    await _guideCrudService.InsertAsync(new Guide
                    {
                        Name = guide.Name,
                        FullDayRate = guide.FullDayRate,
                        HalfDayRate = guide.HalfDayRate,
                        CreatedBy = user.Id
                    });

                    notification.Type = "success";
                    notification.Message = "Guide created successfully.";
                }

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Guide creation failed.";
                return Json(notification);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                notification = new Notification();
                notification = DeleteGuide(id);

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Guide deletion failed.";
                return Json(notification);
            }
        }

        private async Task<Notification> EditGuide(Guide guide, IdentityUser user)
        {
            try
            {
                var record = await _guideCrudService.GetAsync(guide.Id);

                record.Name = guide.Name;
                record.FullDayRate = guide.FullDayRate;
                record.HalfDayRate = guide.HalfDayRate;
                record.ModifiedBy = user.Id;
                record.ModifiedDate = DateTime.Now;

                _guideCrudService.Update(record);

                return new Notification("success", "Guide updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Guide update failed.");
            }
        }

        private Notification DeleteGuide(int id)
        {
            try
            {
                var record = _guideCrudService.Get(id);
                _guideCrudService.Delete(record);

                return new Notification("success", "Guide deleted successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Failed to delete guide.");
            }
        }
    }
}