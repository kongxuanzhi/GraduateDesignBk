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
    [Authorize(Roles = "管理员")]
    public class MsgController : Controller
    {
        // GET: Msg
        public ActionResult Index()
        {
            List<MsgViewModel> msgs = db.Mesg.ToList().Select(m =>
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
                if (db.Mesg.Where(m => m.NID.Equals(id)).Count() > 0)
                {
                    Mesg notice = db.Mesg.Where(m => m.NID.Equals(id)).First();
                    if(notice == null)
                    {
                        return new HttpNotFoundResult();
                    }
                    db.Mesg.Remove(notice);
                }

            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_contextManger != null)
                {
                    _contextManger.Dispose();
                    _contextManger = null;
                }
            }

            base.Dispose(disposing);
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