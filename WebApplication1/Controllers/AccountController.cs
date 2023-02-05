using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Config;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private LinqDataContext db = new LinqDataContext();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        [HttpPost]
        public ActionResult Save(Account req)
        {
            var _acc = db.Accounts.Where(x => x.AccountId == req.AccountId).FirstOrDefault();
            _acc.Email = req.Email;
            _acc.Phone = req.Phone;
            _acc.Balance = req.Balance;
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _taikhoan = db.Accounts.Where(x => x.AccountId == id).FirstOrDefault();
            db.Accounts.DeleteOnSubmit(_taikhoan);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id)
        {
            var _acc = db.Accounts.Where(x => x.AccountId == id).FirstOrDefault();
            return Json(new { success = true, data = _acc }, JsonRequestBehavior.AllowGet);
        }

        // GET: TaiKhoan
        public ActionResult Index()
        {
            ViewBag.ListAccount = db.Accounts.ToList();
            return View();
        }

        public ActionResult Login(Account req)
        {
            var check = db.Accounts.Where(x => x.UserName == req.UserName && x.Password == req.Password);
            if (check.Any())
            {
                var _taikhoan = (from a in db.Accounts.Where(x => x.AccountId == check.FirstOrDefault().AccountId)
                                 select new AccountDTO
                                 {
                                     AccountId = a.AccountId,
                                     UserName = a.UserName,
                                     Password = a.Password,
                                     Email = a.Email,
                                     Phone = a.Phone,
                                     Balance = a.Balance.GetValueOrDefault(),
                                     AccountType = a.AccountType.GetValueOrDefault(),
                                     Admin = a.Admin.GetValueOrDefault(),
                                     Active = a.Active.GetValueOrDefault()
                                 }).FirstOrDefault();
                return Json(new { success = true, data = _taikhoan, token = _taikhoan.UserName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, data = req }, JsonRequestBehavior.AllowGet);
            }
        }

        //public static string createToken(string Username)
        //{
        //    Set issued at date
        //    DateTime issuedAt = DateTime.UtcNow;
        //    đặt thời gian hết hạn token
        //    DateTime expires = DateTime.UtcNow.AddDays(10);

        //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    create a identity and add claims to the user which we want to log in

        //    var userIdentity = new ClaimsIdentity("Identity");
        //    userIdentity.Label = "Identity";
        //    userIdentity.AddClaim(new Claim(ClaimTypes.Name, Username));
        //    userIdentity.AddClaim(new Claim("Username", Username));
        //    userIdentity.AddClaim(new Claim("Category", Category));
        //    userIdentity.HasClaim(ClaimTypes.Role, Category);
        //    var claims = new List<Claim>();

        //    var identity = new ClaimsPrincipal(userIdentity);
        //    Thread.CurrentPrincipal = identity;
        //    string sec = EncryptCode;
        //    string sec = "088881139703564148785";
        //    string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1" + Category;
        //    var now = DateTime.UtcNow;
        //    var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
        //    var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

        //    create the jwt
        //    var token =
        //        (JwtSecurityToken)
        //            tokenHandler.CreateJwtSecurityToken(issuer: "http://unisoft.edu.vn/", audience: "http://unisoft.edu.vn/",
        //                subject: userIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
        //    var tokenString = tokenHandler.WriteToken(token);

        //    return tokenString;
        //}


        [HttpPost]
        public ActionResult FindPassword(Account req)
        {
            var acc = db.Accounts?.Where(x => x.UserName == req.UserName && x.Email == req.Email)?.FirstOrDefault() ?? null;
            if (acc != null)
            {
                var passwordRand = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
                var result = STMP_Config.SendEmail(acc.Email, "TÌM LẠI MẬT KHẨU CHO TÀI KHOẢN : " + req.UserName, "Mật khẩu của bạn đã được đặt lại thành: " + passwordRand, "123123123z", "tiennnth2002007@fpt.edu.vn");
                if (result)
                {
                    acc.Password = passwordRand;
                    db.SubmitChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateInfo(AccountDTO req)
        {
            try
            {
                var acc = db.Accounts.Where(x => x.AccountId == req.AccountId).FirstOrDefault();
                var cus = db.Customers.Where(x => x.AccountId == acc.AccountId).FirstOrDefault();
                cus.CustomerName = req.CustomerName;
                cus.DOB = req.DOB;
                cus.PassPortCode = req.PassPortCode;
                cus.Address = req.Address;
                acc.Email = req.Email;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}