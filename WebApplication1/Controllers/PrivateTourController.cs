using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    public class PrivateTourController : Controller
    {
        // GET: PrivateTour
        private LinqDataContext db = new LinqDataContext();

        public ActionResult Index()
        {
            var listPrivateTour = (from a in db.TourPrivates
                                   select new TourPrivateDTO
                                   {
                                       TourPrivateId = a.TourPrivateId,
                                       LocationFromId = a.LocationFromId,
                                       LocationToId = a.LocationToId,
                                       Slot = a.Slot,
                                       Type = a.Type,
                                       StartDate = a.StartDate,
                                       ToDate = a.ToDate,
                                       HotelId = a.HotelId,
                                       VehicleId = a.VehicleId,
                                       Price = a.Status == 2 ? a.Price : 0,
                                       LocationFrom = db.Locations.Where(x => x.LocationId == a.LocationFromId).FirstOrDefault().LocationName ?? "",
                                       LocationTo = db.Locations.Where(x => x.LocationId == a.LocationToId).FirstOrDefault().LocationName ?? "",
                                       HotelName = db.Hotels.Where(x => x.HotelId == a.HotelId).FirstOrDefault().HotelName ?? "",
                                       VehicleName = db.Vehicles.Where(x => x.VehicleId == a.VehicleId).FirstOrDefault().VahicleName ?? "",
                                       StatusName = a.Status == 1 ? "Đang chờ duyệt" : a.Status == 2 ? "Duyệt thành công" : "Từ chối duyệt",
                                       Status = a.Status,
                                       AccountName = db.Accounts.Where(x => x.AccountId == a.AccountId).FirstOrDefault().UserName ?? ""
                                   }).ToList();
            ViewBag.ListTourPrivates = listPrivateTour; 
            return View();
        }

        [HttpPost]
        public ActionResult UpdateStatus(TourPrivate req)
        {
            try
            {
                var _t = db.TourPrivates.Where(M => M.TourPrivateId == req.TourPrivateId).FirstOrDefault();
                _t.Status = req.Status;
                if (req.Status == 2)
                {
                    _t.Price = req.Price;
                }
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}