using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeaceInternational.Core.Entity;
using PeaceInternational.Core.IRepository;
using PeaceInternational.Web.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PeaceInternational.Web.Controllers
{
    [Authorize]
    public class TourcostController : Controller
    {
        private readonly ICrudService<Tourcost> _tourcostCrudService;
        private readonly ICrudService<TourcostDetail> _tourcostDetailCrudService;
        private readonly ICrudService<Guide> _guideCrudService;
        private readonly ICrudService<Sector> _sectorCrudService;
        private readonly ICrudService<Hotel> _hotelCrudService;
        private readonly ICrudService<SectorTransport> _sectorTransportCrudService;
        private readonly ICrudService<HotelRoomRate> _hotelRoomRateCrudService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;        

        public TourcostController(
            ICrudService<Tourcost> tourcostCrudService,
            ICrudService<TourcostDetail> tourcostDetailCrudService,
            ICrudService<Guide> guideCrudService,
            ICrudService<Sector> sectorCrudService,
            ICrudService<Hotel> hotelCrudService,
            ICrudService<SectorTransport> sectorTransportCrudService,
            ICrudService<HotelRoomRate> hotelRoomRateCrudService,
            IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager)
        {
            _tourcostCrudService = tourcostCrudService;
            _tourcostDetailCrudService = tourcostDetailCrudService;
            _guideCrudService = guideCrudService;
            _sectorCrudService = sectorCrudService;
            _hotelCrudService = hotelCrudService;
            _sectorTransportCrudService = sectorTransportCrudService;
            _hotelRoomRateCrudService = hotelRoomRateCrudService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var tourcost = await _tourcostCrudService.GetAllAsync();
            return View(tourcost.OrderByDescending(p => p.CreatedDate));
        }


        public async Task<IActionResult> View(int id)
        {
            try
            {
                Tourcost tourcost = _tourcostCrudService.Get(id);

                var user = await _userManager.FindByIdAsync(tourcost.CreatedBy);
                tourcost.CreatedBy = user.UserName;

                tourcost.TourcostDetail = _tourcostDetailCrudService.GetAll(p => p.TourcostId == id).ToList();
                tourcost.Guide = _guideCrudService.Get(p => p.Id == tourcost.GuideId);

                foreach(var tourdetail in tourcost.TourcostDetail)
                {
                    tourdetail.Sector1 = _sectorCrudService.Get(tourdetail.Sector1Id);
                    tourdetail.Sector1.SectorTransport = _sectorTransportCrudService.GetAll(p => p.SectorId == tourdetail.Sector1Id).ToList();

                    if (tourdetail.Sector2Id != null)
                    {
                        tourdetail.Sector2 = _sectorCrudService.Get(tourdetail.Sector2Id);
                        tourdetail.Sector2.SectorTransport = _sectorTransportCrudService.GetAll(p => p.SectorId == tourdetail.Sector2Id).ToList();
                    }
                    if (tourdetail.Sector3Id != null)
                    {
                        tourdetail.Sector3.SectorTransport = _sectorTransportCrudService.GetAll(p => p.SectorId == tourdetail.Sector3Id).ToList();
                        tourdetail.Sector3 = _sectorCrudService.Get(tourdetail.Sector3Id);
                    }

                    tourdetail.HotelA = _hotelCrudService.Get(tourdetail.HotelAId);
                    tourdetail.HotelB = _hotelCrudService.Get(tourdetail.HotelBId);
                    tourdetail.HotelC = _hotelCrudService.Get(tourdetail.HotelCId);

                    tourdetail.HotelA.HotelRoomRate = _hotelRoomRateCrudService.Get(p => p.HotelId == tourdetail.HotelAId);
                    tourdetail.HotelB.HotelRoomRate = _hotelRoomRateCrudService.Get(p => p.HotelId == tourdetail.HotelBId);
                    tourdetail.HotelC.HotelRoomRate = _hotelRoomRateCrudService.Get(p => p.HotelId == tourdetail.HotelCId);
                }

                return View(tourcost);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        public async Task<IActionResult> AddEdit(int? id)
        {
            TourcostDTO tourcost = new TourcostDTO();

            if (id > 0)
            {
                tourcost.Tourcost = await _tourcostCrudService.GetAsync(id);
                tourcost.TourcostDetail = _tourcostDetailCrudService.GetAll(p => p.TourcostId == id).ToList();
            }

            ViewBag.Guide = new SelectList(await _guideCrudService.GetAllAsync(), "Id", "Name");
            ViewBag.Sector = new SelectList(await _sectorCrudService.GetAllAsync(), "Id", "Name");
            ViewBag.HotelCatA = new SelectList(await _hotelCrudService.GetAllAsync(p => p.Category == 'A'), "Id", "Code");
            ViewBag.HotelCatB = new SelectList(await _hotelCrudService.GetAllAsync(p => p.Category == 'B'), "Id", "Code");
            ViewBag.HotelCatC = new SelectList(await _hotelCrudService.GetAllAsync(p => p.Category == 'C'), "Id", "Code");

            return View(tourcost);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(TourcostDTO tourcostDTO)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (tourcostDTO.Tourcost.Id > 0)
                {
                    await EditTourcost(tourcostDTO, user);
                }
                else
                {

                    tourcostDTO.Tourcost.MealType = tourcostDTO.Tourcost.MealType ?? "None";
                    tourcostDTO.Tourcost.CreatedBy = user.Id;
                    var tourCostId = await _tourcostCrudService.InsertAsync(tourcostDTO.Tourcost);

                    foreach (var detail in tourcostDTO.TourcostDetail)
                    {
                        detail.TourcostId = tourCostId;
                        detail.CreatedBy = user.Id;

                        await _tourcostDetailCrudService.InsertAsync(detail);
                    }

                }

                _unitOfWork.Commit();
            }
            catch (Exception exception)
            {
                _unitOfWork.Rollback();
                throw exception;
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task EditTourcost(TourcostDTO tourcostDTO, IdentityUser user)
        {
            var tourcost = await _tourcostCrudService.GetAsync(tourcostDTO.Tourcost.Id);

            tourcost.Days = tourcostDTO.Tourcost.Days;
            tourcost.ClientName = tourcostDTO.Tourcost.ClientName;
            tourcost.MinPAX = tourcostDTO.Tourcost.MinPAX;
            tourcost.MaxPAX = tourcostDTO.Tourcost.MaxPAX;
            tourcost.Guide = tourcostDTO.Tourcost.Guide;
            tourcost.IsLuxury = tourcostDTO.Tourcost.IsLuxury;
            tourcost.MealType = tourcostDTO.Tourcost.MealType;
            tourcost.Category1 = tourcostDTO.Tourcost.Category1;
            tourcost.Category2 = tourcostDTO.Tourcost.Category2;
            tourcost.Category3 = tourcostDTO.Tourcost.Category3;
            tourcost.Category4 = tourcostDTO.Tourcost.Category4;
            tourcost.GuideType = tourcostDTO.Tourcost.GuideType;
            tourcost.DiscountAccomodation = tourcostDTO.Tourcost.DiscountAccomodation;
            tourcost.DiscountTransportation = tourcostDTO.Tourcost.DiscountTransportation;
            tourcost.Comment = tourcostDTO.Tourcost.Comment;
            tourcost.ModifiedBy = user.Id;
            tourcost.ModifiedDate = DateTime.Now;

            _tourcostCrudService.Update(tourcost);

            var tourcostDetails = _tourcostDetailCrudService.GetAll(p => p.TourcostId == tourcost.Id);

            foreach (TourcostDetail tourcostDetail in tourcostDetails)
            {
                if (!tourcostDTO.TourcostDetail.Any(x => x.Id == tourcostDetail.Id))
                {
                    _tourcostDetailCrudService.Delete(tourcostDetail);
                }
            }

            foreach (TourcostDetail tourcostDetail in tourcostDTO.TourcostDetail)
            {

                if(tourcostDetail.Id > 0)
                {
                    var record = _tourcostDetailCrudService.Get(tourcostDetail.Id);

                    record.Day = tourcostDetail.Day;
                    record.Sector1Id = tourcostDetail.Sector1Id;
                    record.Sector2Id = tourcostDetail.Sector2Id;
                    record.Sector3Id = tourcostDetail.Sector3Id;
                    record.HotelAId = tourcostDetail.HotelAId;
                    record.HotelBId = tourcostDetail.HotelBId;
                    record.HotelCId = tourcostDetail.HotelCId;
                    record.Category1Cost = tourcostDetail.Category1Cost;
                    record.Category2Cost = tourcostDetail.Category2Cost;
                    record.Category3Cost = tourcostDetail.Category3Cost;
                    record.Category4Cost = tourcostDetail.Category4Cost;
                    record.ModifiedBy = user.Id;
                    record.ModifiedDate = DateTime.Now;

                    _tourcostDetailCrudService.Update(record);
                }
                else
                {
                    tourcostDetail.TourcostId = tourcostDTO.Tourcost.Id;
                    tourcostDetail.CreatedBy = user.Id;

                    await _tourcostDetailCrudService.InsertAsync(tourcostDetail);
                }
            }
        }
    }
}
