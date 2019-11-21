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
    public class InvoiceController : Controller
    {
        private readonly ICrudService<Invoice> _invoiceCrudService;
        private readonly ICrudService<InvoiceDetail> _invoiceDetailCrudService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private Notification notification;

        public InvoiceController(
            ICrudService<Invoice> invoiceCrudService,
            UserManager<IdentityUser> userManager,
            ICrudService<InvoiceDetail> invoiceDetailCrudService,
            IUnitOfWork unitOfWork)
        {
            _invoiceCrudService = invoiceCrudService;
            _invoiceDetailCrudService = invoiceDetailCrudService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //GET Invoice
        [HttpGet]
        public async Task<IActionResult> GetInvoice(int? id)
        {
            try
            {
                if (id == null)
                {
                    var result = await _invoiceCrudService.GetAllAsync();
                    return Json(result);
                }
                else
                {
                    var result = _invoiceCrudService.Get(id);
                    return Json(result);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //GET Invoice with Detail
        [HttpGet]
        public IActionResult GetInvoiceInfo(int? id)
        {
            try
            {
                var invoice = _invoiceCrudService.Get(id);
                var invoiceDetail = _invoiceDetailCrudService.GetAll(p => p.InvoiceId == id).OrderByDescending(p => p.CreatedDate);
                var result = new
                {
                    invoice,
                    invoiceDetail
                };
                return Json(result);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //GET Invoice Detail
        [HttpGet]
        public async Task<IActionResult> GetInvoiceDetail(int invoiceId)
        {
            try
            {
                var result = await _invoiceDetailCrudService.GetAllAsync(p => p.InvoiceId == invoiceId);
                return Json(result);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //Save InvoiceInfo and InvoiceDetail
        [HttpPost]
        public async Task<IActionResult> Save(Invoice invoice)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                notification = new Notification();

                if (invoice.Id > 0)
                {

                    notification = await EditInvoice(invoice, user);
                }
                else
                {

                    int invoiceCount = _invoiceCrudService.GetAll(p => p.InvoiceNo.Contains(invoice.InvoiceNo)).Count();

                    _unitOfWork.BeginTransaction();

                    int InvoiceId = await _invoiceCrudService.InsertAsync(new Invoice
                    {
                        InvoiceNo = $"{invoice.InvoiceNo}/{(invoiceCount+1).ToString().PadLeft(4, '0')}",
                        FileCodeNo = 123,
                        ReferenceNo = invoice.ReferenceNo,
                        Dr = invoice.Dr,
                        AgentName = invoice.AgentName,
                        ClientName = invoice.ClientName,
                        Currency = invoice.Currency,
                        PAX = invoice.PAX,
                        TotalDue = invoice.TotalDue,
                        Discount = invoice.Discount,
                        NetAmount = invoice.NetAmount,
                        CreatedBy = user.Id
                    });

                    foreach(InvoiceDetail invoiceDetail in invoice.InvoiceDetails)
                    {
                        await _invoiceDetailCrudService.InsertAsync(new InvoiceDetail
                        {
                            InvoiceId = InvoiceId,
                            Particulars = invoiceDetail.Particulars,
                            Amount = invoiceDetail.Amount,
                            CreatedBy = user.Id
                        });
                    }

                    _unitOfWork.Commit();

                    notification.Type = "success";
                    notification.Message = "Invoice successfully created.";
                }

                return Json(notification);
            }
            catch (Exception exception)
            {
                _unitOfWork.Rollback();

                notification.Type = "error";
                notification.Message = "Invoice failed to create.";

                return Json(notification);
            }
        }

        private async Task<Notification> EditInvoice(Invoice invoice, IdentityUser user)
        {
            try
            {
                var record = await _invoiceCrudService.GetAsync(invoice.Id);

                record.ReferenceNo = invoice.ReferenceNo;
                record.Dr = invoice.Dr;
                record.AgentName = invoice.AgentName;
                record.ClientName = invoice.ClientName;
                record.Currency = invoice.Currency;
                record.PAX = invoice.PAX;
                record.ModifiedBy = user.Id;
                record.ModifiedDate = DateTime.Now;

                _invoiceCrudService.Update(record);

                return new Notification("success", "Invoice updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new Notification("error", "Invoice update failed.");
            }
        }
    }
}
