using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    public class CustomerController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(Customer req)
        {
            if (req.CustomerId > 0)
            {
                var _customer = db.Customers.Where(x => x.CustomerId == req.CustomerId).FirstOrDefault();
                _customer.CustomerName = req.CustomerName;
                _customer.DOB = req.DOB;
                _customer.PassPortCode = req.PassPortCode;
                _customer.Gender = req.Gender;
                _customer.Address = req.Address;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            db.Customers.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true, data = req }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(AccountDTO req)
        {
            var _acc = new Account();
            var _customer = new Customer();

            _acc.UserName = req.UserName;
            _acc.Password = req.Password;
            _acc.Email = req.Email;
            _acc.Phone = req.Phone;
            _acc.AccountType = 2;
            _acc.Admin = req.Admin;
            _acc.Active = req.Active;
            _acc.Balance = 0;
            db.Accounts.InsertOnSubmit(_acc);
            db.SubmitChanges();

            _customer.CustomerName = req.CustomerName;
            _customer.DOB = req.DOB;
            _customer.PassPortCode = req.PassPortCode;
            _customer.Gender = req.Gender;
            _customer.Address = req.Address;
            _customer.AccountId = _acc.AccountId;
            db.Customers.InsertOnSubmit(_customer);
            db.SubmitChanges();
            return Json(new { success = true, data = req }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _customer = db.Customers.Where(M => M.CustomerId == id).FirstOrDefault();
            db.Customers.DeleteOnSubmit(_customer);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id)
        {
            var _cus = (from a in db.Customers.Where(x => x.CustomerId == id)
                              select new CustomerDTO
                              {
                                  CustomerId = a.CustomerId,
                                  CustomerName = a.CustomerName,
                                  DOB = a.DOB,
                                  PassPortCode = a.PassPortCode,
                                  Gender = a.Gender,
                                  Address = a.Address,
                                  AccountId = a.AccountId,
                                  UserName = db.Accounts.Where(x => x.AccountId == a.AccountId).FirstOrDefault().UserName ?? "Không xác định",
                                  GenderName = a.Gender == 1 ? "Nam" : "Nữ"
                              }).FirstOrDefault();
            return Json(new { success = true, data = _cus }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var listCustomer = (from a in db.Customers.Where(k => db.Accounts.Where(t => t.AccountId == k.AccountId).FirstOrDefault().AccountType == 2)
                                 select new CustomerDTO
                                 {
                                     CustomerId = a.CustomerId,
                                     CustomerName = a.CustomerName,
                                     DOB = a.DOB,
                                     PassPortCode = a.PassPortCode,
                                     Gender = a.Gender,
                                     Address = a.Address,
                                     AccountId = a.AccountId,
                                     UserName = db.Accounts.Where(x => x.AccountId == a.AccountId).FirstOrDefault().UserName ?? "Không xác định",
                                     GenderName = a.Gender == 1 ? "Nam" : "Nữ"
                                 }).ToList();
            ViewBag.ListCustomer = listCustomer;
            return View();
        }
    }
}