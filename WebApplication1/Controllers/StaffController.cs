using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    public class StaffController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(StaffDTO req)
        {
            if (req.StaffId > 0)
            {
                var _staff = db.Staffs.Where(x => x.StaffId == req.StaffId).FirstOrDefault();
                _staff.StaffName = req.StaffName;
                _staff.StaffCode = req.StaffCode;
                _staff.DOB = req.DOB;
                _staff.Gender = req.Gender;
                _staff.Address = req.Address;
                _staff.StartDate = req.StartDate;
                _staff.Position = req.Position;
                _staff.Salary = req.Salary;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            var _acc = new Account();
            _acc.UserName = req.Account.UserName;
            _acc.Password = req.Account.Password;
            _acc.Email = req.Account.Email;
            _acc.Phone = req.Account.Phone;
            _acc.AccountType = 3;
            _acc.Admin = req.Account.Admin;
            _acc.Active = req.Account.Active;
            _acc.Balance = 0;

            db.Accounts.InsertOnSubmit(_acc);
            db.SubmitChanges();

            var _staffInsert = new Staff();
            _staffInsert.StaffName = req.StaffName;
            _staffInsert.StaffCode = req.StaffCode;
            _staffInsert.DOB = req.DOB;
            _staffInsert.Gender = req.Gender;
            _staffInsert.Address = req.Address;
            _staffInsert.StartDate = req.StartDate;
            _staffInsert.Position = req.Position;
            _staffInsert.Salary = req.Salary;
            _staffInsert.AccountId = _acc.AccountId;

            db.Staffs.InsertOnSubmit(_staffInsert);
            db.SubmitChanges();
            return Json(new { success = true, data = req }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _staff = db.Staffs.Where(M => M.StaffId == id).FirstOrDefault();
            db.Staffs.DeleteOnSubmit(_staff);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id)
        {
            var _staff = (from a in db.Staffs.Where(x => x.StaffId == id)
                              select new StaffDTO
                              {
                                  StaffId = a.StaffId,
                                  StaffName = a.StaffName,
                                  StaffCode = a.StaffCode,
                                  DOB = a.DOB,
                                  Gender = a.Gender,
                                  Address = a.Address,
                                  StartDate = a.StartDate,
                                  Position = a.Position,
                                  Salary = a.Salary,
                                  AccountId = a.AccountId,
                                  UserName = db.Accounts.Where(x => x.AccountId == a.AccountId).FirstOrDefault().UserName ?? "Không xác định",
                                  GenderName = a.Gender == 1 ? "Nam" : "Nữ"
                              }).FirstOrDefault();
            return Json(new { success = true, data = _staff }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var listStaff = (from a in db.Staffs.Where(k => db.Accounts.Where(t => t.AccountId == k.AccountId).FirstOrDefault().AccountType == 3)
                                select new StaffDTO
                                {
                                    StaffId = a.StaffId,
                                    StaffName = a.StaffName,
                                    StaffCode = a.StaffCode,
                                    DOB = a.DOB,
                                    Gender = a.Gender,
                                    Address = a.Address,
                                    StartDate = a.StartDate,
                                    Position = a.Position,
                                    Salary = a.Salary,
                                    AccountId = a.AccountId,
                                    UserName = db.Accounts.Where(x => x.AccountId == a.AccountId).FirstOrDefault().UserName ?? "Không xác định",
                                    GenderName = a.Gender == 1 ? "Nam" : "Nữ"
                                }).ToList();
            ViewBag.ListStaff = listStaff;
            return View();
        }

    }
}