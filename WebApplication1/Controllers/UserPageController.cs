using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    public class UserPageController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: UserPage
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuyTicket(OrderDTO req)
        {
            var customerId = db.Customers.Where(x => x.AccountId == req.AccountId).FirstOrDefault().CustomerId;
            var ticketGrowup = db.Tickets.Where(x => x.TicketType == 2).FirstOrDefault().Price ?? 0;
            var ticketChild = db.Tickets.Where(x => x.TicketType == 1).FirstOrDefault().Price ?? 0;

            req.CreatedAt = DateTime.Now;
            req.QRCode = JsonConvert.SerializeObject(req);
            req.CustomerId = customerId;
            var _tour = db.Tours.Where(x => x.TourId == req.TourId).FirstOrDefault();
            var _order = new Order();
            _order.Status = req.Status;
            _order.CreatedAt = DateTime.Now;
            _order.QRCode = req.QRCode;
            _order.CustomerId = req.CustomerId;
            _order.TotalPrice = ticketGrowup * (req.TicketGrowup ?? 0) + ticketChild * (req.TicketChild ?? 0) + (_tour.Price ?? 0);
            _order.TicketGrowup = req.TicketGrowup ?? 0;
            _order.TicketChild = req.TicketChild ?? 0;
            _order.TourId = req.TourId;

            if (db.Accounts.Where(x => x.AccountId == req.AccountId).FirstOrDefault().Balance - _order.TotalPrice < 0)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            db.Orders.InsertOnSubmit(_order);
            db.SubmitChanges();

            var tk = db.Accounts.Where(x => x.AccountId == req.AccountId).FirstOrDefault();
            tk.Balance -= _order.TotalPrice;
            db.SubmitChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListTour(string tourName = "", DateTime? startDate = null, DateTime? endDate = null, string priceOrder = "", string locationFrom = "", string locationTo = "", string vehicleName = "", double startPrice = 0, double endPrice = 0)
        {
            var lstTour = (from a in db.Tours
                           select new TourDTO
                           {
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
            if (!String.IsNullOrEmpty(tourName))
            {
                lstTour = lstTour.Where(x => x.TourName.ToLower().Contains(tourName.ToLower()));
            }
            if (!String.IsNullOrEmpty(locationFrom))
            {
                lstTour = lstTour.Where(x => x.LocationFrom.ToLower().Contains(locationFrom.ToLower()));
            }
            if (!String.IsNullOrEmpty(locationTo))
            {
                lstTour = lstTour.Where(x => x.LocationTo.ToLower().Contains(locationTo.ToLower()));
            }
            if (!String.IsNullOrEmpty(vehicleName))
            {
                lstTour = lstTour.Where(x => x.VehicleName.ToLower().Contains(vehicleName.ToLower()));
            }
            if (!String.IsNullOrEmpty(priceOrder))
            {
                if (priceOrder.Equals("ADSC"))
                {
                    lstTour = lstTour.OrderBy(x => x.Price);

                }
                else
                {
                    lstTour = lstTour.OrderByDescending(x => x.Price);
                }
            }
            if (startDate != null)
            {
                lstTour = lstTour.Where(x => x.StartDate >= startDate);
            }
            if (endDate != null)
            {
                lstTour = lstTour.Where(x => x.EndDate <= endDate);
            }
            if (startPrice > 0)
            {
                lstTour = lstTour.Where(x => x.Price >= startPrice);
            }
            if (endPrice > 0)
            {
                lstTour = lstTour.Where(x => x.Price <= endPrice);
            }

            ViewBag.ListTour = lstTour.ToList();
            return View();
        }

        public ActionResult ListHotel()
        {
            ViewBag.ListHotel = db.Hotels.ToList();
            return View();

        }
        public ActionResult TourDetail(int tourId = 0)
        {
            var _tour = (from a in db.Tours.Where(x => x.TourId == tourId)
                         select new TourDTO
                         {
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
                         }).FirstOrDefault();
            ViewBag.Tour = _tour;
            ViewBag.TourImage = db.TourImages.Where(x => x.TourId == tourId).ToList();
            ViewBag.TourSchedule = db.TourSchedules.Where(x => x.TourId == tourId).ToList();
            var listVote = (from a in db.VoteTours
                            select new VoteTourDTO
                            {
                                VoteTourId = a.VoteTourId,
                                AccountId = a.AccountId,
                                TourId = a.TourId,
                                Star = a.Star,
                                Comment = a.Comment,
                                CreatedAt = a.CreatedAt,
                                UserName = db.Accounts.Where(x => x.AccountId == a.AccountId).FirstOrDefault().UserName ?? ""
                            }
                            ).Where(t => t.TourId == tourId).ToList();
            ViewBag.ListVote = listVote.OrderByDescending(x => x.VoteTourId).ToList();
            return View();
        }

        public ActionResult ListLocation()
        {
            ViewBag.ListLocation = db.Locations.ToList();
            return View();
        }

        public ActionResult BookingTicket(int id = 0)
        {
            var _tour = (from a in db.Tours.Where(x => x.TourId == id)
                         select new TourDTO
                         {
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
                         }).FirstOrDefault();
            ViewBag.Tour = _tour;
            ViewBag.TourId = id;
            ViewBag.ListTicket = db.Tickets.Where(x => x.TourId == id).ToList() ?? new List<Ticket>();
            return View();
        }

        public ActionResult Location(int id = 0)
        {
            var _location = db.Locations.Where(x => x.LocationId == id).FirstOrDefault();
            return Json(new { success = true, data = _location }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListTicket(int account_id)
        {
            var ma = db.Customers.Where(x => x.AccountId == account_id).FirstOrDefault().CustomerId;
            ViewBag.ListTicket = db.Orders.Where(x => x.CustomerId == account_id).ToList() ?? null;
            ViewBag.Customer = db.Customers.Where(x => x.AccountId == account_id).FirstOrDefault() ?? null;
            ViewBag.Balance = db.Accounts.Where(x => x.AccountId == account_id).FirstOrDefault().Balance ?? 0;
            return View();
        }

        public ActionResult NapTien(int id_tk)
        {
            var tk = db.Accounts.Where(x => x.AccountId == id_tk).FirstOrDefault();
            tk.Balance = tk.Balance > 0 ? tk.Balance : 0;
            tk.Balance += 5000000;
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelOrder(int id)
        {
            try
            {
                var _order = db.Orders.Where(x => x.OrderId == id).FirstOrDefault();

                if (_order.CreatedAt.GetValueOrDefault().AddDays(3) > DateTime.Now)
                {
                    var customerId = _order.CustomerId;
                    var acc_id = db.Customers.Where(x => x.CustomerId == customerId).FirstOrDefault().AccountId;
                    var tk = db.Accounts.Where(x => x.AccountId == acc_id).FirstOrDefault();
                    tk.Balance = tk.Balance > 0 ? tk.Balance : 0;
                    tk.Balance += _order.TotalPrice;
                    _order.Status = 2;
                    _order.DeletedAt = DateTime.Now;
                    db.SubmitChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else if (_order.CreatedAt.GetValueOrDefault().AddDays(5) > DateTime.Now)
                {
                    var customerId = _order.CustomerId;
                    var acc_id = db.Customers.Where(x => x.CustomerId == customerId).FirstOrDefault().AccountId;
                    var tk = db.Accounts.Where(x => x.AccountId == acc_id).FirstOrDefault();
                    tk.Balance = tk.Balance > 0 ? tk.Balance : 0;
                    tk.Balance += _order.TotalPrice * (float)(70/100);
                    _order.Status = 2;
                    _order.DeletedAt = DateTime.Now;
                    db.SubmitChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else if (_order.CreatedAt.GetValueOrDefault().AddDays(8) > DateTime.Now)
                {
                    var customerId = _order.CustomerId;
                    var acc_id = db.Customers.Where(x => x.CustomerId == customerId).FirstOrDefault().AccountId;
                    var tk = db.Accounts.Where(x => x.AccountId == acc_id).FirstOrDefault();
                    tk.Balance = tk.Balance > 0 ? tk.Balance : 0;
                    tk.Balance += _order.TotalPrice/2;
                    _order.Status = 2;
                    _order.DeletedAt = DateTime.Now;
                    db.SubmitChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Bạn chỉ có thể hủy tour trong vòng 3 ngày từ lúc đặt vé." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}