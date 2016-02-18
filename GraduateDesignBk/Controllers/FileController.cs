using GraduateDesignBk.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System;

namespace GraduateDesignBk.Controllers
{
    public class FileController : Controller
    {
        private static int FilePageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FilePageSize"]);
        // GET: File
        /// <summary>
        /// 文件列表 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult Index(FileSandP file)
        {
            int pageSize = (int)file.page.PageSize + 8;
            file.FileItems = getFiles();
            //过滤
            file.FileItems = file.FileItems
                .Where(m => m.Name.Contains(CNTS(file.Sname))
                 && m.FromUID.Contains(CNTS(file.SuserName))
                 && m.Pub == file.Spub
                 && m.Type.Contains(CNTS(file.Stype))
                ).ToList();

            //分页
            file.page.TotalCount = file.FileItems.Count();
            file.page.PageNum = (int)Math.Ceiling((double)(file.page.TotalCount) / pageSize);
            file.FileItems = file.FileItems.OrderByDescending(m => m.UploadTime).Skip(pageSize * (file.page.CurIndex - 1)).Take(pageSize).ToList();
            return View(file);
        }

        public string CNTS(string value)
        {
            return value == null ? "" : value;
        }


        public JsonResult List(int SearchType, string SearchString,int CurIndex = 1)
        {
            FileSandP file = new FileSandP();
            file.page.FilePageSize = FilePageSize;
            file.page.CurIndex = CurIndex;

            file.FileItems = getFiles();
            file.FileItems = file.FileItems
                .Where(m => m.Pub == true).ToList();
            switch (SearchType)
            {
                case 0:
                    file.FileItems = file.FileItems.Where(m => m.Name.Contains(CNTS(SearchString))).ToList();
                    break;
                case 1:
                    file.FileItems = file.FileItems.Where(m => m.FromUID.Contains(CNTS(SearchString))).ToList();
                    break;
                case 2:
                    file.FileItems = file.FileItems.Where(m => m.UploadTime.Equals(SearchString)).ToList();
                    break;
                default:
                    break;
            }
            file.page.TotalCount = file.FileItems.Count();
            file.page.PageNum = (int)Math.Ceiling((double)(file.page.TotalCount) / FilePageSize);
            file.FileItems = file.FileItems.OrderByDescending(m => m.UploadTime).Skip(FilePageSize * (file.page.CurIndex - 1)).Take(FilePageSize).ToList();
            return Json(file);
        }

        public List<FileViewModel> getFiles()
          {
            List<FileViewModel> fileItems = new List<FileViewModel>();
            fileItems  = db.File.ToList().Select(m =>
                new FileViewModel()
                {
                    FID = m.FID,
                    Name = m.Name,
                    FromId = m.FromUID,
                    Pub = m.Pub,
                    UploadTime = m.UploadTime.ToString("yyyy-MM-dd"),
                    Type = m.Type,
                    Size = m.Size,
                    DownloadTimes = m.DownloadTimes,
                    FromUID = UserManager.FindById(m.FromUID).RealName
                }
            ).ToList();
            return fileItems;
        }

        /// <summary>
        /// 文件详情 文件基本信息，接收人，如果是老师同学之间发送，就是私密发送，教师和管理员可以公开
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            FileDetail filed = new FileDetail();
            if(db.File.Where(m => m.FID.Equals(id)).Count() > 0)
            {
                filed.file  = db.File.ToList().Select(m =>
                   new FileViewModel()
                   {
                       FID = m.FID,
                       Name = m.Name,
                       FromId = m.FromUID,
                       Pub = m.Pub,
                       UploadTime = m.UploadTime.ToShortDateString(),
                       Type = m.Type,
                       Size = m.Size,
                       DownloadTimes = m.DownloadTimes,
                       FromUID = UserManager.FindById(m.FromUID).RealName
                   }).First();
                List<string> ToUIDs = db.DownUpload.Where(m=>m.FID.Equals(id)).Select(m => m.ToUID).ToList();
                filed.Receives = UserManager.Users.ToList().Where(m => ToUIDs.Contains(m.Id)).ToList();
                return View(filed);
            }
            return new HttpNotFoundResult();
        }
        
        //上传文件 公开上传到前台，供大家下载
        [HttpPost]
        public ActionResult FileUpload(bool Pub)
        {
            HttpPostedFileBase file = Request.Files["file"];
            if (file != null)
            {
                File mfile = new Models.File()
                {
                    Name = file.FileName,
                    Pub = Pub,
                    FromUID = User.Identity.GetUserId(),
                    Type = System.IO.Path.GetExtension(file.FileName),
                    Size = ChangeSize(file.ContentLength),
                    
                };
                string fileName = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + mfile.FID.Substring(0, 5) + System.IO.Path.GetExtension(file.FileName);
                string filePath = System.IO.Path.Combine(HttpContext.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["FilePath"]), System.IO.Path.GetFileName(fileName));
                file.SaveAs(filePath);

                mfile.FileSeq = fileName;
                db.File.Add(mfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public FileStreamResult Download(string id)
        {
            string absoluFilePath;
            if (db.File.Where(m => m.FID.Equals(id)).Count() > 0)
            {
                File file = db.File.Where(m => m.FID.Equals(id)).First();
                absoluFilePath = System.IO.Path.Combine(HttpContext.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["FilePath"]), System.IO.Path.GetFileName(file.FileSeq));
                FileStreamResult filee =  File(new System.IO.FileStream(absoluFilePath, System.IO.FileMode.Open), "application/octet-stream", Server.UrlEncode(file.FileSeq));
                filee.FileDownloadName = file.Name;
                return filee;
            }
            return null;
        }

        public string ChangeSize(int B)
        {
            return   B < 1024 ? B + "b" :
                     (B< 1048576 && B>=1024) ? Math.Round((double)B / 1024,2) + "kb" :
                     (B >= 1048576) ? Math.Round((double)B / (1024 * 1024)) + "Mb" : B + "";
        }
        /// <summary>
        ///批量删除文件 并且从数据库中删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public ActionResult  DeleteMany(string Ids)
        {
            string[] ids = Ids.Split(new char[] { '|'});
            foreach (string  id in ids)
            {
                File file = db.File.Where(m=>m.FID.Equals(id)).First();
                if (file == null)
                {
                    return new HttpNotFoundResult();
                }
                string filePath = System.IO.Path.Combine(HttpContext.Server.MapPath("../files"), file.FileSeq);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                db.File.Remove(file);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 批量公开文件 支持前台下载
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public ActionResult PubMany(string Ids)
        {
            string[] ids = Ids.Split(new char[] { '|' });
            foreach (string id in ids)
            {
                File file = db.File.Where(m => m.FID.Equals(id)).First();
                file.Pub = true;
                if (file == null)
                {
                    return new HttpNotFoundResult();
                }
                TryUpdateModel(file);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 批量隐藏文件
        /// </summary>
        /// <param name="Ids">隐藏文件的id字符串</param>
        /// <returns></returns>
        public ActionResult HideMany(string Ids)
        {
            string[] ids = Ids.Split(new char[] { '|' });
            foreach (string id in ids)
            {
                File file = db.File.Where(m => m.FID.Equals(id)).First();
                file.Pub = false;
                if (file == null)
                {
                    return new HttpNotFoundResult();
                }
                TryUpdateModel(file);
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