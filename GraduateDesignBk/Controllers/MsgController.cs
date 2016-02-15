using GraduateDesignBk.Models;
using GraduateDesignBk.Models.ViewsModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraduateDesignBk.Controllers
{
    public class MsgController : Controller
    {
        // GET: Msg
        public ActionResult Index()
        {
            List<MsgViewModel> msgs = db.Notice.ToList().Select(m =>
                new MsgViewModel()
                {
                    NID = m.NID,
                    CreateTime = m.CreateTime,
                    Detail = m.Detail,
                    FromId = m.FromUID,
                    Title = m.Title,
                    FromUID = UserManager.FindById(m.FromUID).UserName
                }
            ).ToList();
           return View(msgs);
        }

        public ActionResult DeleteMany(string Id)
        {
            string[] ids = Id.Split('|');
            foreach (string id in ids)
            {
                if (db.Notice.Where(m => m.NID.Equals(id)).Count() > 0)
                {
                    Notice notice = db.Notice.Where(m => m.NID.Equals(id)).First();
                    if(notice == null)
                    {
                        return new HttpNotFoundResult();
                    }
                    db.Notice.Remove(notice);
                }

            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region 初始化
        private ApplicationUserManager _userManager;
        public ApplicationDbContext _contextManger;
        public ApplicationDbContext db
        {
            get
            {
                return _contextManger ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _contextManger = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }  
            private set
            {
                _userManager = value;
            }
        }
        #endregion
    }
}