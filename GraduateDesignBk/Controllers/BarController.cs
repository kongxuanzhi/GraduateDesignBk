using GraduateDesignBk.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System;
using GraduateDesignBk;
using GraduateDesignBk.App_Start;

namespace Graduatedesignbk.Controllers
{
    public class BarController : Controller
    {
        private static int pagesize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["pagesize"]);
        #region 显示
        // get: bar
        public ActionResult index(BarViewModel barv)
        {
            int pagesize = (int)barv.page.PageSize + 8;

            barv.Bars.pbars = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail").OrderBy(m => m.RaiseQuesTime).ToList();
            barv.Bars.pbars = barv.Bars.pbars.Where(m => m.FromName.Contains(cnts(barv.SAuthor))).ToList();
            barv.Bars.pbars = barv.Bars.pbars.Where(m => m.Title.Contains(cnts(barv.SQue)) || m.Description.Contains(cnts(barv.SQue))).ToList();

            barv.Bars.fbars = db.Database.SqlQuery<AnswerDetail>("select * from V_AnswerDetail  where FAID='0'").OrderBy(m => m.AnswerQuesTime).ToList();
            barv.Bars.sbars = db.Database.SqlQuery<AnswerDetail>("select * from V_AnswerDetail  where FAID<>'0'").OrderBy(m => m.AnswerQuesTime).ToList();

            if (barv.isPub != 0)
            {
                if ((int)barv.isPub == 1)
                    barv.Bars.pbars = barv.Bars.pbars.Where(m => m.Pub == true).ToList();
                if ((int)barv.isPub == 2)
                    barv.Bars.pbars = barv.Bars.pbars.Where(m => m.Pub == false).ToList();
            }
            barv.page.TotalCount = barv.Bars.pbars.Count();
            barv.page.PageNum = (int)Math.Ceiling((double)(barv.page.TotalCount) / pagesize);
            barv.Bars.pbars = barv.Bars.pbars.OrderByDescending(m => m.RaiseQuesTime).Skip(pagesize * (barv.page.CurIndex - 1)).Take(pagesize).ToList();
            return View(barv);
        }

        //public ActionResult everydayques()
        //{
        //    barViewmodel barv = new barViewmodel();
        //    barv.Bars.pbars = db.Database.SqlQuery<bardetail>("select * from v_bars_users Where touid is null and fbid=pbid and fbid='0'").OrderBy(m => m.raisequestime).ToList();
        //    barv.Bars.pbars = barv.Bars.pbars.Where(m => m.raisequestime.toshortdatestring().equals(DateTime.now.toshortdatestring())).ToList();

        //    barv.Bars.fbars = db.Database.SqlQuery<bardetail>("select * from v_bars_users vb Where  vb.pbid=vb.fbid and  vb.fbid <> '0'").OrderBy(m => m.raisequestime).ToList();
        //    barv.Bars.sbars = db.Database.SqlQuery<bardetail>("select * from v_bars_users vb Where  vb.pbid <>vb.fbid").OrderBy(m => m.raisequestime).ToList();
        //    return View(barv);
        //}
        public ActionResult Statisics()
        {
            return View();
        }
        #endregion

        #region 前台显示

        [HttpPost]
        [LoginAuthorize(Roles ="教师")]
        public JsonResult Search(string searchstr,int currentIndex = 1)
        {
            searchstr = string.IsNullOrEmpty(searchstr)?Guid.NewGuid().ToString() : searchstr;
            List<QestDetail> SearchQues = new List<QestDetail>();
            SearchQues = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail where ToUID='0'").ToList();
            SearchQues = SearchQues.Where(m => m.Title.Contains(searchstr) || m.Description.Contains(searchstr)).ToList();
            SearchQues = SearchQues.OrderByDescending(m => m.Likes).Skip(pagesize * (currentIndex - 1)).Take(pagesize).ToList();
            return Json(SearchQues);
        }
        [HttpPost]
        public JsonResult Latest(int currentIndex = 1)
        {
            List<QestDetail> LatestQues = new List<QestDetail>();
            LatestQues = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail where ToUID='0'").ToList();
            LatestQues = LatestQues.Where(m=>m.RaiseQuesTime.ToShortDateString().Equals(DateTime.Now.ToShortDateString())).ToList();
           LatestQues = LatestQues.OrderByDescending(m => m.RaiseQuesTime).Skip(pagesize * (currentIndex - 1)).Take(pagesize).ToList();
            return Json(LatestQues);
        }
        [HttpPost]
        public JsonResult Hotest(int currentIndex = 1)
        {
            List<QestDetail> HotestQues = new List<QestDetail>();
            HotestQues = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail VQ where ToUID='0' and VQ.Likes+CommentNum>10 order by VQ.Likes+CommentNum   desc").ToList();
            HotestQues = HotestQues.OrderByDescending(m => m.RaiseQuesTime).Skip(pagesize * (currentIndex - 1)).Take(pagesize).ToList();
            return Json(HotestQues);
        }
        [HttpPost]
        public JsonResult UnAns(int currentIndex = 1)
        {
            List<QestDetail> UnAnsQues = new List<QestDetail>();
            UnAnsQues = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail where  ToUID='0' and CommentNum=0").ToList();
            UnAnsQues = UnAnsQues.OrderByDescending(m => m.RaiseQuesTime).Skip(pagesize * (currentIndex - 1)).Take(pagesize).ToList();
            return Json(UnAnsQues);
        }

        [HttpPost]
        public JsonResult BestQuesAns()
        {
            List<QestDetail> BestQues = new List<QestDetail>();
            BestQues = db.Database.SqlQuery<QestDetail>("select top 10 * from V_QestDetail where ToUID != '0' and Pub=1 order by RaiseQuesTime desc").ToList();
            BestQues = BestQues.OrderByDescending(m => m.RaiseQuesTime).ToList();
            return Json(BestQues);
        }

        #endregion
        #region 发表、私信、评论、追问 删除问题，评论，追问

        //发表提问
        [HttpPost]
        [LoginAuthorize]
        public JsonResult Post(string Title, string Description)
        {
            string str = "logined|";
            if (string.IsNullOrEmpty(Title)) return Json(str+"标题必填！");
            if (string.IsNullOrEmpty(Description)) return Json(str+"请尽可能详细的描述您的问题！");
            Question ques = new Question()
            {
                FromUID = User.Identity.GetUserId(),
                ToUID ="0",
                Pub = true,
                Title = Title,
                Description = Description,
            };
            db.Questions.Add(ques);
            db.SaveChanges();
            return Json(str+"发表成功!");
        }
        //私信
        public JsonResult PrivateLetter(string TeacherId,string Title, string Description)
        {
            Question ques = new Question() {
                FromUID = User.Identity.GetUserId(),
                ToUID = TeacherId,
                Pub = false,
                Title = Title,
                Description = Description,
            };
            db.Questions.Add(ques);
            db.SaveChanges();
            return Json("success");
        }
        //评论 第一次回复
        public JsonResult Comment(string QID, string Description)
        {
            Question Ques = db.Questions.Find(QID);
            if (Ques != null)
            {
                Answer ans = new Answer()
                {
                    FromUID = User.Identity.GetUserId(),
                    ToUID = Ques.FromUID,
                    PQID = Ques.QID,
                    Description = Description,
                 };
                Ques.CommentNum++;
                TryUpdateModel(Ques);
                db.Answers.Add(ans);
                db.SaveChanges();
                return Json("success");
            }
            return Json("error");
        }
        //追问                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        public JsonResult Reply(string AID,string Description)
        {
            Answer ansCom = db.Answers.Find(AID);
            if (ansCom != null)
            {
                Answer reply = new Answer() {
                    FromUID = User.Identity.GetUserId(),
                    ToUID = ansCom.FromUID,
                    FAID = ansCom.AID,
                    PQID = ansCom.PQID,
                    Description = Description,
                };
                string QID = ansCom.PQID;
                Question que = db.Questions.Find(QID);
                que.CommentNum++;
                TryUpdateModel(que);
                db.Answers.Add(reply);
                db.SaveChanges();
                return Json("success");
            }
            return Json("error");
        }

        public JsonResult DeleteQues(string id)
        {
            string[] ids = id.Split('|');
            foreach (string QID in ids)
            {
                Question que = db.Questions.Find(QID);
                if (que == null)
                {
                    return Json("error");
                }
                db.Questions.Remove(que);
                List<Answer> anses = db.Answers.Where(m => m.PQID.Equals(QID)).ToList();
                db.Answers.RemoveRange(anses);
                db.SaveChanges();
            }
            return Json("success");
        }
        public JsonResult DeleteAnswer(string AID)
        {
            Answer ans = db.Answers.Find(AID);
            if (ans != null)
            {
                if (ans.FAID.Equals("0")) //删除该评论下的所有追问
                {
                    List<Answer> ansons = db.Answers.Where(m => m.FAID.Equals(ans.AID)).ToList();
                    db.Answers.RemoveRange(ansons);
                }
                else
                {
                    db.Answers.Remove(ans);   //删除该追问
                }
                db.SaveChanges();
                return Json("success");
            }
            return Json("error");
        }
        #endregion

        #region 为问题、回答点赞 ，设置为问题已解决设置为公开或私密

        //ajax为问题点赞
        [HttpPost]
        [LoginAuthorize]
        public JsonResult QuestLike(string QID)
        {
            string userId = User.Identity.GetUserId();
            Question ques = db.Questions.Find(QID);
            if (ques != null)
            {
                int count = db.LikeOnce.Where(m => m.BID == QID && m.FromUID == userId).Count();
                if (count==0)
                {
                    //增加点赞记录
                    db.LikeOnce.Add(new likeOnce() { BID = QID, FromUID = userId });
                    ques.Likes += 1;
                    TryUpdateModel(ques);
                    db.SaveChanges();
                    return Json("addsuccess");
                }else if (count==1)
                {
                    likeOnce lo = db.LikeOnce.Where(m => m.BID == QID && m.FromUID == userId).First();
                    //增加点赞记录
                    db.LikeOnce.Remove(lo);
                    ques.Likes -= 1;
                    TryUpdateModel(ques);
                    db.SaveChanges();
                    return Json("delsuccess");
                }
            }
            return Json("error");
        }
       
        //ajax为问题点赞
        public string AddAnsLike(string AID)
        {
            Answer ans = db.Answers.Find(AID);
            if (ans != null)
            {
                var search = new { BID = AID, FromUID = User.Identity.GetUserId() };
                likeOnce lo = db.LikeOnce.Find(search);
                if (lo == null)
                {
                    //增加点赞记录
                    db.LikeOnce.Add(new likeOnce() { BID = AID, FromUID = User.Identity.GetUserId() });
                    ans.Likes += 1;
                    TryUpdateModel(ans);
                    db.SaveChanges();
                    return "success";
                }
                return "error_exist";
            }
            return "error";
        }
        //取消问题赞
        public string DeleteAnsLike(string AID)
        {
            Answer ans = db.Answers.Find(AID);
            if (ans != null)
            {
                var search = new { BID = AID, FromUID = User.Identity.GetUserId() };
                likeOnce lo = db.LikeOnce.Find(search);
                if (lo != null)
                {
                    //增加点赞记录
                    db.LikeOnce.Remove(lo);
                    ans.Likes -= 1;
                    TryUpdateModel(ans);
                    db.SaveChanges();
                    return "success";
                }
                return "error_exist";
            }
            return "error";
        }
        public string SolvedQues(string QID)
        {
            Question que = db.Questions.Find(QID);
            if (que != null)
            {
                que.Solved = true;
                TryUpdateModel(que);
                db.SaveChanges();
                return "success";
            }
            return "error";
        }
        public JsonResult PubQues(string QID)
        {
            Question que = db.Questions.Find(QID);
            if (que != null)
            {
                que.Pub = true;
                TryUpdateModel(que);
                db.SaveChanges();
                return Json("success");
            }
            return Json("error");
        }
        public JsonResult PrivQues(string QID)
        {
            Question que = db.Questions.Find(QID);
            if (que != null)
            {
                que.Pub = false;
                TryUpdateModel(que);
                db.SaveChanges();
                return Json("success");
            }
            return Json("error");
        }
        #endregion
        
        #region 统计
        public JsonResult ThisWeek()
        {
            List<string> week = getpast7days();
            int[] quesN = getPast7QuesN();
            int[] ansTN = getPast7Anses("教师");
            int[] ansSN = getPast7Anses("学生");
            double[] ansTRate = getAnsRate(ansTN,quesN);
            double[] ansSRate = getAnsRate(ansSN,quesN);
            JsonResult result = new JsonResult
            {
                Data = new
                {
                    Ques = quesN,
                    AnsTN = ansTN,
                    AnsSN = ansSN, 
                    AnsTRate = ansTRate,
                    AnsSRate = ansSRate,
                    Week = week
                }
            };
            return result;
        }

        private double[] getAnsRate(int[] ansN, int[] quesN)
        {
            double[] rate = new double[7];
            for (int i = 0; i < ansN.Length; i++)
            {
                rate[i] = quesN[i] == 0 ? 0 : (double)ansN[i] / quesN[i];
            }
            return rate;
        }

        public int[] getPast7Anses(string role)
        {
            int[] ansN = new int[7];
            DateTime dt = DateTime.Now;
            for (int i = 0; i < 7; i++)
            {
                List<Answer> anses = db.Answers.ToList().Where(m => m.AnswerQuesTime.ToShortDateString().Equals(dt.ToShortDateString())).ToList();
                anses = anses.Where(m => UserManager.IsInRole(m.FromUID, role)).ToList();
                ansN[7 - i - 1] = anses.Count();
                dt = dt.AddDays(-1);
            }
            return ansN;
        }

        public int[] getPast7QuesN()
        {
            int[] quesN = new int[7];
            DateTime dt = DateTime.Now;
            for (int i = 0; i < 7; i++)
            {
                List<Question> queses = db.Questions.ToList().Where(m => m.RaiseQuesTime.ToShortDateString().Equals(dt.ToShortDateString())).ToList();
                quesN[7-i-1] = queses.Count();
                dt = dt.AddDays(-1);
            }
            return quesN;
        }

        public List<string> getpast7days()
        {
            DateTime dt = DateTime.Now;
            List<string> week = new List<string>();
            int day = (int)dt.DayOfWeek + 7;
            week.Add(dt.DayOfWeek.ToString());
            for (int i = 0; i < 6; i++)
            {
                DayOfWeek dow = (DayOfWeek)((day - 1) % 7);
                week.Add(dow.ToString());
                day--;
            }
            week.Reverse();
            return week;
        }

        public class perday
        {
            public string teachername { get; set; }
            public int quesnum { get; set; }
        }

        public JsonResult todayteacherans()
        {
            //sqlparameter p = new sqlparameter("@t","教师");
            //to.teachers = db.Database.SqlQuery<string>("select realname from v_user_role Where name =@t",p).ToList();
            //int[] a = new int[] { 12,  1 };
            //to.quesnum.addrange(a);
            JsonResult result = new JsonResult
            {
                //data = t
            };
            return result;
        }

       
        #endregion

        #region Helper

        public string cnts(string value)
        {
            return value == null ? "" : value;
        }
        #endregion

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