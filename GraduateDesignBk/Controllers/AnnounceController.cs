using GraduateDesignBk.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraduateDesignBk.Controllers
{
    public class AnnounceController : Controller
    {
        // GET: Announce
        public ActionResult Index(AnnouandP Annou)
        {
            int pageSize = (int)Annou.page.PageSize + 8;
            Annou.AnnouItems = getAnnous();
            //过滤
            Annou.AnnouItems = Annou.AnnouItems
                .Where(m => m.Title.Contains(CNTS(Annou.STitle)) || m.Content.Contains(CNTS(Annou.STitle))
                 && m.FromUID.Contains(CNTS(Annou.SuserName))
                ).ToList();

            //分页
            Annou.page.TotalCount = Annou.AnnouItems.Count();
            Annou.page.PageNum = (int)Math.Ceiling((double)(Annou.page.TotalCount) / pageSize);
            Annou.AnnouItems = Annou.AnnouItems.OrderBy(m => m.Time).Skip(pageSize * (Annou.page.CurIndex - 1)).Take(pageSize).ToList();
            return View(Annou);
        }
        public string CNTS(string value)
        {
            return value == null ? "" : value;
        }
        public List<AnnounceView> getAnnous()
        {
            List<AnnounceView> AnnouItems = new List<AnnounceView>();
            AnnouItems = db.Announces.ToList().Select(m =>
               new AnnounceView()
               {
                   ANID = m.ANID,
                   Title = m.Title,
                   FromUID = m.FromUID,
                   Time = m.Time,
                   FromName = UserManager.FindById(m.FromUID).RealName
               }
            ).ToList();
            return AnnouItems;
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