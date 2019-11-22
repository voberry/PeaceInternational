using System;
using System.Linq;
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
    public class HotelReceiptController : Controller
    {

        private readonly ICrudService<HotelReceipt> _hotelReceiptCrudService;
        private readonly ICrudService<Hotel> _hotelCrudService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private Notification notification;

        public HotelReceiptController(
            ICrudService<HotelReceipt> hotelReceiptCrudService,
            ICrudService<Hotel> hotelCrudService,
            UserManager<IdentityUser> userManager,           
            IUnitOfWork unitOfWork)
        {
            _hotelReceiptCrudService = hotelReceiptCrudService;
            _hotelCrudService = hotelCrudService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //GET HotelReciept
        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                if (id == null)
                {
                    var result = await _hotelReceiptCrudService.GetAll()
                        .AsNoTracking()
                        .Include(h => h.Hotel)
                        .ToListAsync();
                    return Json(result.OrderByDescending(h => h.CreatedDate));
                }
                else
                {
                    var result = _hotelReceiptCrudService.Get(id);
                    return Json(result);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [HttpGet]
        public IActionResult GetServiceVoucher(int id)
        {
            try
            {
                var hotelReceipt = _hotelReceiptCrudService.Get(id);
                var hotel = _hotelCrudService.Get(hotelReceipt.HotelId);
                var result = new
                {
                    hotelReceipt,
                    hotel
                };
                return Json(result);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //Save HotelReceipt
        [HttpPost]
        public async Task<IActionResult> Save(HotelReceipt hotelReceipt)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                notification = new Notification();

                if (hotelReceipt.Id > 0)
                {

                    notification = await EditHotelReceipt(hotelReceipt, user);
                }
                else
                {
                    await _hotelReceiptCrudService.InsertAsync(new HotelReceipt
                    {
                        ExchangeOrderNo = hotelReceipt.ExchangeOrderNo,
                        FileCodeNo = hotelReceipt.FileCodeNo,
                        HotelId = hotelReceipt.HotelId,
                        ClientName = hotelReceipt.ClientName,
                        PAX = hotelReceipt.PAX,
                        ArrivalDate = hotelReceipt.ArrivalDate,
                        From = hotelReceipt.From,
                        ArrivalFlight = hotelReceipt.ArrivalFlight,
                        DepartureDate = hotelReceipt.DepartureDate,
                        To = hotelReceipt.To,
                        DepartureFlight = hotelReceipt.DepartureFlight,
                        Services = hotelReceipt.Services,
                        CreatedBy = user.Id
                    });

                    notification.Type = "success";
                    notification.Message = "Hotel Receipt created successfully.";
                }

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Hotel Receipt creation failed.";
                return Json(notification);
            }
        }

        //Edit Hotel Receipt
        private async Task<Notification> EditHotelReceipt(HotelReceipt hotelReceipt, IdentityUser user)
        {
            try
            {
                var record = await _hotelReceiptCrudService.GetAsync(hotelReceipt.Id);

                record.ExchangeOrderNo = hotelReceipt.FileCodeNo;
                record.FileCodeNo = hotelReceipt.FileCodeNo;
                record.HotelId = hotelReceipt.HotelId;
                record.ClientName = hotelReceipt.ClientName;
                record.PAX = hotelReceipt.PAX;
                record.ArrivalDate = hotelReceipt.ArrivalDate;
                record.From = hotelReceipt.From;
                record.ArrivalFlight = hotelReceipt.ArrivalFlight;
                record.DepartureDate = hotelReceipt.DepartureDate;
                record.To = hotelReceipt.To;
                record.DepartureFlight = hotelReceipt.DepartureFlight;
                record.Services = hotelReceipt.Services;
                record.ModifiedBy = user.Id;
                record.ModifiedDate = DateTime.Now;

                _hotelReceiptCrudService.Update(record);

                return new Notification("success", "Hotel Receipt updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Hotel Receipt update failed.");
            }
        }
    }
}
