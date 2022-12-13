using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HotelController : Controller
    {
        // GET: Hotel
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(Hotel req)
        {
            if (req.HotelId > 0)
            {
                var _hotel = db.Hotels.Where(x => x.HotelId == req.HotelId).FirstOrDefault();
                _hotel.HotelName = req.HotelName;
                _hotel.TypeRoom = req.TypeRoom;
                _hotel.PricePerOne = req.PricePerOne;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            db.Hotels.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _hotel = db.Hotels.Where(M => M.HotelId == id).FirstOrDefault();
            db.Hotels.DeleteOnSubmit(_hotel);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id)
        {
            var _hotel = db.Hotels.Where(M => M.HotelId == id).FirstOrDefault();
            return Json(new { success = true, data = _hotel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var listHotel = db.Hotels.ToList();
            ViewBag.ListHotel = listHotel;
            return View();
        }
    }
}