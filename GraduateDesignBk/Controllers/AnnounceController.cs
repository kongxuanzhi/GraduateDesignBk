using GraduateDesignBk.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GraduateDesignBk.Controllers
{
    public class AnnounceController : Controller
    {
        private static int AnnouncePageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AnnouncePageSize"]);
        // GET: Announce
        public ActionResult Index(AnnouandP Annou)
        {
            Annou.page.IPageSize = (int)Annou.page.PageSize + 8;
            return View(ListAnnounce(Annou));
        }

        [HttpPost]
        public JsonResult AllAnnounce(AnnouandP Annou)
        {
            Annou.page.IPageSize = AnnouncePageSize;
            return Json(ListAnnounce(Annou)); 
        }
        public AnnouandP ListAnnounce(AnnouandP Annou)
        {
            Annou.AnnouItems = getAnnous();
            //过滤
            Annou.AnnouItems = Annou.AnnouItems
                .Where(m => m.Title.Contains(CNTS(Annou.STitle))
                 && m.FromName.Contains(CNTS(Annou.SuserName))
                ).ToList();
            //分页
            Annou.page.TotalCount = Annou.AnnouItems.Count();
            Annou.page.PageNum = (int)Math.Ceiling((double)(Annou.page.TotalCount) / Annou.page.IPageSize);
            Annou.AnnouItems = Annou.AnnouItems.OrderByDescending(m => m.Prop).ThenByDescending(m=>m.Time).Skip(Annou.page.IPageSize * (Annou.page.CurIndex - 1)).Take(Annou.page.IPageSize).ToList();
            return Annou;
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
                   Time = m.Time.ToString(),
                   Prop = m.Prop,
                   ReadTimes = m.ReadTimes,
                   FromName = UserManager.FindById(m.FromUID).RealName
               }
            ).ToList();
            return AnnouItems;
        }
        //设置为置顶
        public  async Task<ActionResult> SetTop(string Ids)
        {
            string[] ids = Ids.Split('|');
            foreach(string ANID in ids)
            {
                Announce an = await db.Announces.FindAsync(ANID);
                if (an == null)
                {
                    return View("error");
                }
                an.Prop = 2;
                TryUpdateModel(an);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //取消置顶
        public async Task<ActionResult> DeleteTop(string Ids)
        {
            string[] ids = Ids.Split('|');
            foreach (string ANID in ids)
            {
                Announce an = await db.Announces.FindAsync(ANID);
                if (an == null)
                {
                    return View("error");
                }
                an.Prop = 0;
                TryUpdateModel(an);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Latest10()
        {
            List<AnnounceView> Avs = db.Announces.ToList().Select(m =>
                new AnnounceView()
                {
                    ANID = m.ANID,
                    Title = m.Title,
                    Time = m.Time.Year == DateTime.Now.Year ? m.Time.ToString("MM-dd") : m.Time.ToString("yyyy-MM-dd") 
                }
            ).OrderBy(m=>m.Time).Skip(0).Take(10).ToList();
            return Json(Avs);
        }

        public ActionResult Post()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Post(PostAnnounceView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Content.Length == 0)
            {
                ModelState.AddModelError("", "请填写公告内容");
                return View(model);
            }
            Announce an = new Announce()
            {
                Title = model.Title,
                Content = model.Content,
                FromUID = User.Identity.GetUserId()
            };
            db.Announces.Add(an);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string Ids)
        {
            string[] ids = Ids.Split('|');
            foreach (string ANID in ids)
            {
                Announce an = await db.Announces.FindAsync(ANID);
                if (an == null)
                {
                    return View("error");
                }
                db.Announces.Remove(an);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string ANID)
        {
            if (string.IsNullOrEmpty(ANID))
            {
                return RedirectToAction("Index");
            }
            Announce model = await db.Announces.FindAsync(ANID);
            if (model != null)
            {
                PostAnnounceView pmodel = new PostAnnounceView()
                {
                    ANID = model.ANID, Title = model.Title, Content = model.Content
                };
                return View(pmodel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PostAnnounceView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Announce m = await db.Announces.FindAsync(model.ANID);
            if (m != null)
            {
                m.Content = model.Content;
                m.FromUID = User.Identity.GetUserId();
                m.Title = model.Title;
                m.Time = DateTime.Now;
                TryUpdateModel(m);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        
        public async Task<ActionResult> Detail(string ANID)
        {
            Announce m = await db.Announces.FindAsync(ANID);
            if (m != null)
            {
                m.ReadTimes++;  //增加阅读量
                TryUpdateModel(m);
                await db.SaveChangesAsync();
                AnnounceView av = new AnnounceView()
                {
                    ANID = m.ANID, Title = m.Title,
                    Content = m.Content, Time = m.Time.ToString(),
                    FromName = UserManager.FindById(m.FromUID).RealName,
                    FromUID = m.FromUID,
                    ReadTimes = m.ReadTimes
                };
                 return View(av);
            }
            return View("Error","公告不存在");
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