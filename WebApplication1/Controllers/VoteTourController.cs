using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VoteTourController : Controller
    {
        // GET: VoteTour
        public ActionResult Index()
        {
            return View();
        }

        // GET: Category
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        public ActionResult Save(VoteTour req)
        {
            req.CreatedAt = DateTime.Now;
            db.VoteTours.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var _vote = db.VoteTours.Where(M => M.VoteTourId == id).FirstOrDefault();
            db.VoteTours.DeleteOnSubmit(_vote);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}