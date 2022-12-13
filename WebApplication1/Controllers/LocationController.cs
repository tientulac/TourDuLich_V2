using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LocationController : Controller
    {
        // GET: Location
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(Location req)
        {
            if (req.LocationId > 0)
            {
                var _location = db.Locations.Where(x => x.LocationId == req.LocationId).FirstOrDefault();
                _location.LocationCode = req.LocationCode;
                _location.LocationName = req.LocationName;
                _location.Locate = req.Locate;
                _location.Iframe = req.Iframe;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            db.Locations.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _location = db.Locations.Where(M => M.LocationId == id).FirstOrDefault();
            db.Locations.DeleteOnSubmit(_location);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id)
        {
            var _location = db.Locations.Where(M => M.LocationId == id).FirstOrDefault();
            return Json(new { success = true, data = _location }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var listLocation = db.Locations.ToList();
            ViewBag.ListLocation = listLocation;
            return View();
        }
    }
}