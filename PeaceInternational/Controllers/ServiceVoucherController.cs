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
    public class ServiceVoucherController : Controller
    {

        private readonly ICrudService<ServiceVoucher> _serviceVoucherCrudService;
        private readonly ICrudService<Hotel> _hotelCrudService;
        private readonly ICrudService<FiscalYear> _fiscalYearCrudService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private Notification notification;

        public ServiceVoucherController(
            ICrudService<ServiceVoucher> serviceVoucherCrudService,
            ICrudService<Hotel> hotelCrudService,
            UserManager<IdentityUser> userManager,
            ICrudService<FiscalYear> fiscalYearCrudService,
            IUnitOfWork unitOfWork)
        {
            _serviceVoucherCrudService = serviceVoucherCrudService;
            _hotelCrudService = hotelCrudService;
            _userManager = userManager;
            _fiscalYearCrudService = fiscalYearCrudService;
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
                    var result = await _serviceVoucherCrudService.GetAll()
                        .AsNoTracking()
                        .Include(h => h.Hotel)
                        .ToListAsync();
                    return Json(result.OrderByDescending(h => h.CreatedDate));
                }
                else
                {
                    var result = _serviceVoucherCrudService.Get(id);
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
                var serviceVoucher = _serviceVoucherCrudService.Get(id);
                var hotel = _hotelCrudService.Get(serviceVoucher.HotelId);
                var user = _userManager.FindByIdAsync(serviceVoucher.CreatedBy).Result;
                var result = new
                {
                    serviceVoucher,
                    hotel,
                    user.UserName
                };
                return Json(result);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //Save ServiceVoucher
        [HttpPost]
        public async Task<IActionResult> Save(ServiceVoucher serviceVoucher)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                var currentFiscalYear = _fiscalYearCrudService.Get(p => DateTime.Now.Date >= p.StartDateAD && DateTime.Now.Date <= p.EndDateAD);

                notification = new Notification();

                if (serviceVoucher.Id > 0)
                {

                    notification = await EditServiceVoucher(serviceVoucher, user);
                }
                else
                {
                    await _serviceVoucherCrudService.InsertAsync(new ServiceVoucher
                    {
                        ExchangeOrderNo = GetExchangeOrderNo(currentFiscalYear),
                        FileCodeNo = serviceVoucher.FileCodeNo,
                        FiscalYearId = currentFiscalYear.Id,
                        HotelId = serviceVoucher.HotelId,
                        ClientName = serviceVoucher.ClientName,
                        PAX = serviceVoucher.PAX,
                        ArrivalDate = serviceVoucher.ArrivalDate,
                        From = serviceVoucher.From,
                        ArrivalFlight = serviceVoucher.ArrivalFlight,
                        DepartureDate = serviceVoucher.DepartureDate,
                        To = serviceVoucher.To,
                        DepartureFlight = serviceVoucher.DepartureFlight,
                        Services = serviceVoucher.Services,
                        CreatedBy = user.Id
                    });

                    notification.Type = "success";
                    notification.Message = "Service Voucher created successfully.";
                }

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Service Voucher creation failed.";
                return Json(notification);
            }
        }

        //Edit Service Voucher
        private async Task<Notification> EditServiceVoucher(ServiceVoucher serviceVoucher, IdentityUser user)
        {
            try
            {
                var record = await _serviceVoucherCrudService.GetAsync(serviceVoucher.Id);
                
                record.FileCodeNo = serviceVoucher.FileCodeNo;
                record.HotelId = serviceVoucher.HotelId;
                record.ClientName = serviceVoucher.ClientName;
                record.PAX = serviceVoucher.PAX;
                record.ArrivalDate = serviceVoucher.ArrivalDate;
                record.From = serviceVoucher.From;
                record.ArrivalFlight = serviceVoucher.ArrivalFlight;
                record.DepartureDate = serviceVoucher.DepartureDate;
                record.To = serviceVoucher.To;
                record.DepartureFlight = serviceVoucher.DepartureFlight;
                record.Services = serviceVoucher.Services;
                record.ModifiedBy = user.Id;
                record.ModifiedDate = DateTime.Now;

                _serviceVoucherCrudService.Update(record);

                return new Notification("success", "Service Voucher updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Service Voucher update failed.");
            }
        }

        private string GetExchangeOrderNo(FiscalYear currentFiscalYear)
        {
            var count = _serviceVoucherCrudService.GetAll(p => p.FiscalYearId == currentFiscalYear.Id).Count();
            var formattedFiscalYear = currentFiscalYear.Name.Remove(2, 1);

            if (formattedFiscalYear == "7677")
            {
                count += 248;
            }

            var exchangeOrderNo = $"{formattedFiscalYear}/{(count + 1).ToString().PadLeft(4, '0')}";
            return exchangeOrderNo;
        }
    }
}
