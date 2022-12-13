using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Vehicle
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(Vehicle req)
        {
            if (req.VehicleId > 0)
            {
                var _vehicle = db.Vehicles.Where(x => x.VehicleId == req.VehicleId).FirstOrDefault();
                _vehicle.VahicleName = req.VahicleName;
                _vehicle.VehicleCode = req.VehicleCode;
                _vehicle.Slot = req.Slot;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                req.Status = 1;
            }
            db.Vehicles.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _vehicle = db.Vehicles.Where(M => M.VehicleId == id).FirstOrDefault();
            db.Vehicles.DeleteOnSubmit(_vehicle);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id)
        {
            var _vehicle = db.Vehicles.Where(M => M.VehicleId == id).FirstOrDefault();
            return Json(new { success = true, data = _vehicle }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var listVehicle = db.Vehicles.ToList();
            ViewBag.ListVehicle = listVehicle;
            return View();
        }
    }
}