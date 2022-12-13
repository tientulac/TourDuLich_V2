using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(Order req)
        {
            db.Orders.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _order = db.Orders.Where(M => M.OrderId == id).FirstOrDefault();
            db.Orders.DeleteOnSubmit(_order);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var listOrder = (from a in db.Orders
                              select new OrderDTO
                              {
                                  OrderId = a.OrderId,
                                  TicketId = a.TicketId,
                                  Status = a.Status,
                                  CreatedAt = a.CreatedAt,
                                  DeletedAt = a.DeletedAt,
                                  QRCode = a.QRCode,
                                  CustomerId = a.CustomerId,
                                  StatusName = a.Status == 1 ? "Chưa sử dụng" : "Đã qua sử dụng",
                                  CustomerName = db.Customers.Where(x => x.CustomerId == a.CustomerId).FirstOrDefault().CustomerName ?? ""
                              }).ToList();
            ViewBag.ListOrder = listOrder;
            return View();
        }

    }
}