using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(Category req)
        {
            if (req.CategoryId > 0)
            {
                var _cate = db.Categories.Where(x => x.CategoryId == req.CategoryId).FirstOrDefault();
                _cate.CategoryCode = req.CategoryCode;
                _cate.CategoryName = req.CategoryName;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            db.Categories.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _cate = db.Categories.Where(M => M.CategoryId == id).FirstOrDefault();
            db.Categories.DeleteOnSubmit(_cate);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id)
        {
            var _cate = db.Categories.Where(M => M.CategoryId == id).FirstOrDefault();
            return Json(new { success = true, data = _cate }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var listCategory = db.Categories.ToList();
            ViewBag.ListCategory = listCategory;
            return View();
        }
    }
}