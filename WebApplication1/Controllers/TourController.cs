using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    public class TourController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(Tour req)
        {
            if (req.TourId > 0)
            {
                var _tour = db.Tours.Where(x => x.TourId == req.TourId).FirstOrDefault();
                _tour.TourName = req.TourName;
                _tour.StartDate = req.StartDate;
                _tour.EndDate = req.EndDate;
                _tour.TourTime = req.TourTime;
                _tour.LocationFrom = req.LocationFrom;
                _tour.LocationTo = req.LocationTo;
                _tour.Price = req.Price;
                _tour.Poster = req.Poster;
                _tour.Descrip = req.Descrip;
                _tour.VehicleId = req.VehicleId;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            db.Tours.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _tour = db.Tours.Where(M => M.TourId == id).FirstOrDefault();
            db.Tours.DeleteOnSubmit(_tour);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id)
        {
            var _tour = db.Tours.Where(M => M.TourId == id).FirstOrDefault();
            return Json(new { success = true, data = _tour }, JsonRequestBehavior.AllowGet);
        }

        // GET: Tour
        public ActionResult Index()
        {
            var lstTour = (from a in db.Tours
                           select new TourDTO {
                                TourId = a.TourId,
                                TourName = a.TourName,
                                StartDate = a.StartDate,
                                EndDate = a.EndDate,
                                TourTime = a.TourTime,
                                LocationFrom = a.LocationFrom,
                                LocationTo = a.LocationTo,
                                Price = a.Price,
                                Poster = a.Poster,
                                Descrip = a.Descrip,
                                VehicleId = a.VehicleId,
                                VehicleName = db.Vehicles.Where(x => x.VehicleId == a.VehicleId).FirstOrDefault().VahicleName ?? "",
                           });
            ViewBag.ListVehicle = db.Vehicles.ToList();
            ViewBag.ListTour = lstTour;
            return View();
        }

        public ActionResult GetListImage(int id)
        {
            var lstImage = db.TourImages.Where(M => M.TourId == id).ToList();
            return Json(new { success = true, data = lstImage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListSchedule(int id)
        {
            var lstSchedule = db.TourSchedules.Where(M => M.TourId == id).ToList();
            return Json(new { success = true, data = lstSchedule }, JsonRequestBehavior.AllowGet);
        }
    }
}