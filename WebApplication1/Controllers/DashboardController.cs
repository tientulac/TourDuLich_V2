using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    public class DashboardController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            ViewBag.CountTour = db.Tours.ToList().Count();
            ViewBag.CountCustomer = db.Customers.ToList().Count();
            ViewBag.CountVehicle = db.Vehicles.ToList().Count();
            ViewBag.TotalIncome = db.Orders.Where(o => o.TotalPrice > 0).Sum(x => x.TotalPrice) ?? 0;
            ViewBag.CountHoaDon = db.Orders.ToList().Count();
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
            ViewBag.ListVehicle = db.Vehicles.ToList();
            ViewBag.ListTour = lstTour;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}