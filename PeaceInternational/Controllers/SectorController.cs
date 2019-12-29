using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeaceInternational.Core.Entity;
using PeaceInternational.Core.IRepository;
using PeaceInternational.Web.Models;

namespace PeaceInternational.Web.Controllers
{
    [Authorize]
    public class SectorController : Controller
    {
        private readonly ICrudService<Sector> _sectorCrudService;
        private readonly ICrudService<SectorTransport> _sectorTransportCrudService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private Notification notification;

        public SectorController(
            ICrudService<Sector> sectorCrudService,
            UserManager<IdentityUser> userManager,
            ICrudService<SectorTransport> sectorTransportCrudService,
            IUnitOfWork unitOfWork)
        {
            _sectorCrudService = sectorCrudService;
            _sectorTransportCrudService = sectorTransportCrudService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;

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
                    var result =  await _sectorCrudService.GetAll().AsNoTracking()
                        .Include(h => h.SectorTransport)
                        .ToListAsync();                   

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
        public async Task<IActionResult> Save(SectorDTO sectorDTO)
        {
            try
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                notification = new Notification();

                _unitOfWork.BeginTransaction();

                if (sectorDTO.Sector.Id > 0)
                {

                    notification = await EditSector(sectorDTO, user);
                }
                else
                {   

                    int sectorId = await _sectorCrudService.InsertAsync(new Sector
                    {
                        Name = sectorDTO.Sector.Name,
                        Code = sectorDTO.Sector.Code,                     
                        CreatedBy = user.Id
                    });

                    foreach(var sectorTransport in sectorDTO.SectorTransport)
                    {
                        await _sectorTransportCrudService.InsertAsync(new SectorTransport
                        {
                            SectorId = sectorId,
                            TransportId = sectorTransport.TransportId,
                            Cost = sectorTransport.Cost,                          
                            CreatedBy = user.Id
                        });
                    }

                    notification.Type = "success";
                    notification.Message = "Sector created successfully.";
                }

                _unitOfWork.Commit();

                return Json(notification);
            }
            catch (Exception exception)
            {
                _unitOfWork.Rollback();
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

        private async Task<Notification> EditSector(SectorDTO sectorDTO, IdentityUser user)
        {
            try
            {
                var record = await _sectorCrudService.GetAsync(sectorDTO.Sector.Id);

                record.Name = sectorDTO.Sector.Name;
                record.Code = sectorDTO.Sector.Code;           
                record.ModifiedBy = user.Id;
                record.ModifiedDate = DateTime.Now;

                foreach (var sectorTransport in sectorDTO.SectorTransport)
                {
                    var recordST = await _sectorTransportCrudService.GetAsync(p => p.SectorId == sectorDTO.Sector.Id && p.TransportId == sectorTransport.TransportId);

                    recordST.Cost = sectorTransport.Cost;                  
                    recordST.ModifiedBy = user.Id;
                    record.ModifiedDate = DateTime.Now;

                    _sectorTransportCrudService.Update(recordST);
                }

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
                var sectorTransport = _sectorTransportCrudService.GetAll(p => p.SectorId == id);

                _sectorTransportCrudService.Delete(sectorTransport);

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