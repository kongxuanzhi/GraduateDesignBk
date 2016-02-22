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
    public class NoteController : Controller
    {
        [HttpPost]
        public JsonResult Display()
        {
           Note note =  db.Notes.OrderByDescending(m => m.Time).First();
            if(note != null)
            {
                return Json(note.Content);
            }
            return Json("error");
        }

        // GET: Note
        public ActionResult Index()
        {
            List<Note> notes = db.Notes.OrderByDescending(m=>m.Time).ToList();
            return View(notes);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateNoteView NoteV)
        {
            if (!ModelState.IsValid)
            {
                return View(NoteV);
            }
            Note note = new Note() {
                Content = NoteV.Content,
                FromUID = User.Identity.GetUserName()
            };
            db.Notes.Add(note);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteMany(string Id)
        {
            string[] ids = Id.Split('|');
            foreach(string id in ids)
            {
                if (db.Notes.Where(m => m.NTID.Equals(id)).Count() > 0)
                {
                    Note n = db.Notes.Where(m => m.NTID.Equals(id)).First();
                    db.Notes.Remove(n);
                }
                else
                {
                    return new HttpNotFoundResult();
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