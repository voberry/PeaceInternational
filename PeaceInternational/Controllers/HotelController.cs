using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeaceInternational.Core.Entity;
using PeaceInternational.Core.IRepository;
using PeaceInternational.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PeaceInternational.Web.Controllers
{
    [Authorize]
    public class HotelController : Controller
    {
        private readonly ICrudService<Hotel> _hotelCrudService;
        private readonly UserManager<IdentityUser> _userManager;
        private  Notification notification;

        public HotelController(
            ICrudService<Hotel> hotelCrudService,
            UserManager<IdentityUser> userManager)
        {
            _hotelCrudService = hotelCrudService;
            _userManager = userManager;
        }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //GET Hotel
        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                if(id == null)
                {
                    var result = await _hotelCrudService.GetAllAsync();
                    return Json(result);
                }
                else
                {
                    var result = _hotelCrudService.Get(id);
                    return Json(result);
                }
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }
        //Save Hotel
        [HttpPost]
        public async Task<IActionResult> Save(Hotel hotel)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                notification = new Notification();

                if (hotel.Id > 0)
                {
                    
                    notification = await EditHotel(hotel, user);
                }
                else
                {
                    await _hotelCrudService.InsertAsync(new Hotel
                    {
                        Name = hotel.Name,
                        PhoneNo = hotel.PhoneNo,
                        Address = hotel.Address,
                        CreatedBy = user.Id
                    });

                    notification.Type = "success";
                    notification.Message = "Hotel created successfully.";
                }

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Hotel creation failed.";
                return Json(notification);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {               
                notification = new Notification();
                notification = DeleteHotel(id);

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Hotel deletion failed.";
                return Json(notification);
            }
        }

        private async Task<Notification> EditHotel(Hotel hotel, IdentityUser user)
        {
            try
            {
                var record = await _hotelCrudService.GetAsync(hotel.Id);

                record.Name = hotel.Name;
                record.Address = hotel.Address;
                record.PhoneNo = hotel.PhoneNo;
                record.ModifiedBy = user.Id;
                record.ModifiedDate = DateTime.Now;

                _hotelCrudService.Update(record);

                return new Notification("success", "Hotel updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Hotel update failed.");
            }
        }
        private Notification DeleteHotel(int id)
        {
            try
            {
                var record = _hotelCrudService.Get(id);
                _hotelCrudService.Delete(record);

                return new Notification("success", "Hotel deleted successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Failed to delete hotel.");
            }
        }
    }
}
