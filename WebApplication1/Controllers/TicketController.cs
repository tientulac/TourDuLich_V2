using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    public class TicketController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(Ticket req)
        {
            if (req.TicketId > 0)
            {
                var _ticket = db.Tickets.Where(x => x.TicketId == req.TicketId).FirstOrDefault();
                _ticket.Status = req.Status;
                _ticket.TicketType = req.TicketType;
                _ticket.Price = req.Price;
                _ticket.TourId = req.TourId;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            db.Tickets.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _ticket = db.Tickets.Where(M => M.TicketId == id).FirstOrDefault();
            db.Tickets.DeleteOnSubmit(_ticket);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id)
        {
            var _ticket = (from a in db.Tickets
                           select new TicketDTO
                           {
                               TicketId = a.TicketId,
                               Status = a.Status,
                               TicketType = a.TicketType,
                               Price = a.Price,
                               TourId = a.TourId,
                               TourName = db.Tours.Where(x => x.TourId == a.TourId).FirstOrDefault().TourName ?? "Không xác định",
                           }).Where(t => t.TicketId == id).FirstOrDefault();
            return Json(new { success = true, data = _ticket }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var listTicket = (from a in db.Tickets
                            select new TicketDTO
                            {
                                TicketId = a.TicketId,
                                Status = a.Status,
                                TicketType = a.TicketType,
                                Price = a.Price,
                                TourId = a.TourId,
                                TourName = db.Tours.Where(x => x.TourId == a.TourId).FirstOrDefault().TourName ?? "Không xác định",
                            }).ToList();
            var listTour = db.Tours.ToList();
            ViewBag.ListTicket = listTicket;
            ViewBag.ListTour = listTour;
            return View();
        }
    }
}