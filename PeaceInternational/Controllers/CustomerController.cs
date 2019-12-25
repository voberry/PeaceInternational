using System;
using System.Linq;
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
    public class CustomerController : Controller
    {
        private readonly ICrudService<Customer> _customerCrudService;
        private readonly ICrudService<FiscalYear> _fiscalYearCrudService;
        private readonly UserManager<IdentityUser> _userManager;
        private Notification notification;

        public CustomerController(
            ICrudService<Customer> customerCrudService,
            ICrudService<FiscalYear> fiscalYearCrudService,
            UserManager<IdentityUser> userManager)
        {
            _customerCrudService = customerCrudService;
            _fiscalYearCrudService = fiscalYearCrudService;
            _userManager = userManager;
        }
       
        public IActionResult Index()
        {
            return View();
        }

        //GET Customer
        [HttpGet]
        public async Task<IActionResult> Get(string fileCodeNo)
        {
            try
            {
                if (fileCodeNo == null)
                {
                    var result = await _customerCrudService.GetAllAsync();
                    return Json(result.OrderByDescending(p => p.CreatedDate));
                }
                else
                {
                    var result = _customerCrudService.Get(p => p.FileCodeNo == fileCodeNo);
                    return Json(result);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //Save Customer
        [HttpPost]
        public async Task<IActionResult> Save(Customer customer)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                var currentFiscalYear = _fiscalYearCrudService.Get(p => DateTime.Now.Date >= p.StartDateAD && DateTime.Now.Date <= p.EndDateAD);
                notification = new Notification();

                if (customer.Id > 0)
                {

                    notification = await EditCustomer(customer, user);
                }
                else
                {
                    await _customerCrudService.InsertAsync(new Customer
                    {
                        FileCodeNo = GetFileCodeNo(currentFiscalYear),
                        FiscalYearId = currentFiscalYear.Id,
                        TourName = customer.TourName,
                        Country = customer.Country,
                        ArrivalDate = customer.ArrivalDate,
                        DepartureDate = customer.DepartureDate,
                        Agent = customer.Agent,
                        AgentStaff = customer.AgentStaff,
                        GuideName = customer.GuideName,
                        CreatedBy = user.Id
                    });

                    notification.Type = "success";
                    notification.Message = "Customer created successfully.";
                }

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Customer creation failed.";
                return Json(notification);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                notification = new Notification();
                notification = DeleteCustomer(id);

                return Json(notification);
            }
            catch (Exception exception)
            {
                notification.Type = "error";
                notification.Message = "Customer deletion failed.";
                return Json(notification);
            }
        }

        [HttpGet]
        public JsonResult CheckFileCodeNo(string fileCodeNo)
        {
            var customer = _customerCrudService.Get(p => p.FileCodeNo == fileCodeNo);
            return customer == null ? Json(0) : Json(1);
        }

        private async Task<Notification> EditCustomer(Customer customer, IdentityUser user)
        {
            try
            {
                var record = await _customerCrudService.GetAsync(customer.Id);

                record.TourName = customer.TourName;
                record.Country = customer.Country;
                record.ArrivalDate = customer.ArrivalDate;
                record.DepartureDate = customer.DepartureDate;
                record.Agent = customer.Agent;
                record.AgentStaff = customer.AgentStaff;
                record.GuideName = customer.GuideName;
                record.ModifiedDate = DateTime.Now;

                _customerCrudService.Update(record);

                return new Notification("success", "Customer updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Customer update failed.");
            }
        }

        private Notification DeleteCustomer(int id)
        {
            try
            {
                var record = _customerCrudService.Get(id);
                _customerCrudService.Delete(record);

                return new Notification("success", "Customer deleted successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Failed to delete customer.");
            }
        }

        private string GetFileCodeNo(FiscalYear currentFiscalYear)
        {            
            var count = _customerCrudService.GetAll(p => p.FiscalYearId == currentFiscalYear.Id).Count();
            var formattedFiscalYear = currentFiscalYear.Name.Remove(2, 1);

            if (formattedFiscalYear == "7677")
            {
                count += 70;
            }

            var fileCodeNo = $"{formattedFiscalYear}/{(count + 1).ToString().PadLeft(4, '0')}";
            return fileCodeNo;
        }
    }
}
