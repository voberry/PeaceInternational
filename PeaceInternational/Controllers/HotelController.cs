using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICrudService<HotelRoomRate> _hotelRoomRateCrudService;
        private readonly UserManager<IdentityUser> _userManager;
        private  Notification notification;

        public HotelController(
            ICrudService<Hotel> hotelCrudService,
            ICrudService<HotelRoomRate> hotelRoomRateCrudService,
            UserManager<IdentityUser> userManager)
        {
            _hotelCrudService = hotelCrudService;
            _hotelRoomRateCrudService = hotelRoomRateCrudService;
            _userManager = userManager;
        }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("HotelRoomRate")]       
        public IActionResult HotelRoomRate()
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

        //GET Hotel Room Rate
        [HttpGet("HotelRoomRate/Get")]
        public async Task<IActionResult> GetHotelRoomRate(int? id)
        {
            try
            {
                if (id == null)
                {
                    var result = await _hotelRoomRateCrudService.GetAll()
                         .AsNoTracking()
                         .Include(h => h.Hotel)
                         .ToListAsync();
                    return Json(result);
                }
                else
                {
                    var result = _hotelRoomRateCrudService.Get(id);
                    return Json(result);
                }
            }
            catch (Exception exception)
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
                        Code = hotel.Code,
                        Category = hotel.Category,
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

        //Save Hotel Room Rate
        [HttpPost("HotelRoomRate/Save")]
        public async Task<IActionResult> SaveHotelRoomRate(HotelRoomRate hotelRoomRate)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                notification = new Notification();

                if (hotelRoomRate.Id > 0)
                {

                    notification = await EditHotelRoomRate(hotelRoomRate, user);
                }
                else
                {
                    await _hotelRoomRateCrudService.InsertAsync(new HotelRoomRate
                    {
                        HotelId = hotelRoomRate.HotelId,
                        SingleBed = hotelRoomRate.SingleBed,
                        DoubleBed = hotelRoomRate.DoubleBed,
                        ExtraBed = hotelRoomRate.ExtraBed,                      
                        AP = hotelRoomRate.AP,
                        MAP = hotelRoomRate.MAP,
                        CreatedBy = user.Id
                    });

                    notification.Type = "success";
                    notification.Message = "Hotel Room Rate created successfully.";
                }

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Hotel Room Rate creation failed.";
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

        [HttpPost("HotelRoomRate/Delete")]
        public IActionResult DeleteHotelRoomRate(int id)
        {
            try
            {
                notification = new Notification();
                notification = DeleteRoomRate(id);

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Hotel Room Rate deletion failed.";
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
                record.Code = hotel.Code;
                record.Category = hotel.Category;
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

        private async Task<Notification> EditHotelRoomRate(HotelRoomRate hotelRoomRate, IdentityUser user)
        {
            try
            {
                var record = await _hotelRoomRateCrudService.GetAsync(hotelRoomRate.Id);

                record.HotelId = hotelRoomRate.HotelId;
                record.SingleBed = hotelRoomRate.SingleBed;
                record.DoubleBed = hotelRoomRate.DoubleBed;
                record.ExtraBed = hotelRoomRate.ExtraBed;               
                record.AP = hotelRoomRate.AP;
                record.MAP = hotelRoomRate.MAP;
                record.ModifiedBy = user.Id;
                record.ModifiedDate = DateTime.Now;

                _hotelRoomRateCrudService.Update(record);

                return new Notification("success", "Hotel Room Rate updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Hotel Room Rate update failed.");
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

        private Notification DeleteRoomRate(int id)
        {
            try
            {
                var record = _hotelRoomRateCrudService.Get(id);
                _hotelRoomRateCrudService.Delete(record);

                return new Notification("success", "HotelRoomRate deleted successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Failed to delete Hotel Room Rate.");
            }
        }
    }
}
