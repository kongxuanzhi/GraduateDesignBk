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
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;

namespace Graduatedesignbk.Controllers
{
    public class BarController : Controller
    {
        private static int pagesize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["pagesize"]);
        private static int statisPastDays= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["statisPastDays"]);
        #region 显示
        // get: bar
        [Authorize(Roles = "管理员,教师")]
        public ActionResult index(BarViewModel barv)
        {
            int pagesize = (int)barv.page.PageSize + 8;
            barv.Bars.pbars = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail").OrderBy(m => m.RaiseQuesTime).ToList();
            barv.Bars.pbars = barv.Bars.pbars.Where(m => m.FromName.Contains(cnts(barv.SAuthor))).ToList();
            barv.Bars.pbars = barv.Bars.pbars.Where(m => m.Title.Contains(cnts(barv.SQue)) || m.Description.Contains(cnts(barv.SQue))).ToList();
            barv.Bars.pbars = barv.Bars.pbars.Where(m => m.RaiseQuesTime.ToString("yyyy-MM-dd").Contains(cnts(barv.sTime))).ToList();

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

        [HttpPost]
        [AllowAnonymous]
        public JsonResult QDetail(string QID)
        {
            if (!string.IsNullOrEmpty(QID))
            {
                Question q = db.Questions.Find(QID);
                if (q != null)
                {
                    q.ReadTimes++;
                    TryUpdateModel(q);
                    db.SaveChanges();
                }
            }
            BarViewModel barv = new BarViewModel();
            SqlParameter pqid = new SqlParameter("@QID", QID);
            SqlParameter fqid = new SqlParameter("@FQID", QID);
            SqlParameter sqid = new SqlParameter("@SQID", QID);
            barv.Bars.pbars = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail where QID=@QID",pqid).OrderBy(m => m.RaiseQuesTime).ToList();
            barv.Bars.fbars = db.Database.SqlQuery<AnswerDetail>("select * from V_AnswerDetail where PQID=@FQID and FAID='0'", fqid).OrderByDescending(m=>m.Likes).ThenBy(m => m.AnswerQuesTime).ToList();
            barv.Bars.sbars = db.Database.SqlQuery<AnswerDetail>("select * from V_AnswerDetail where PQID=@SQID and FAID<>'0'", sqid).OrderBy(m => m.AnswerQuesTime).ToList();
            return Json(barv);
        }
        public ActionResult Statisics()
        {
            return View();
        }
        #endregion
        #region 前台显示
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Search(string searchstr,int currentIndex = 1)
        {
            searchstr = string.IsNullOrEmpty(searchstr)?Guid.NewGuid().ToString() : searchstr;
            List<QestDetail> SearchQues = new List<QestDetail>();
            SearchQues = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail where ToUID='0'").ToList();
            SearchQues = SearchQues.Where(m => m.Title.Contains(searchstr) || m.Description.Contains(searchstr)).ToList();
            SearchQues = SearchQues.OrderByDescending(m => m.Likes).Skip(pagesize * (currentIndex - 1)).Take(pagesize).ToList();
            return Json(SearchQues);
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult All(int currentIndex = 1)
        {
            List<QestDetail> LatestQues = new List<QestDetail>();
            LatestQues = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail where ToUID='0'").ToList();
            LatestQues = LatestQues.OrderByDescending(m => m.Likes).ThenBy(m=>m.RaiseQuesTime).Skip(pagesize * (currentIndex - 1)).Take(pagesize).ToList();
            return Json(LatestQues);
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Latest(int currentIndex = 1)
        {
            List<QestDetail> LatestQues = new List<QestDetail>();
            LatestQues = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail where ToUID='0'").ToList();
            LatestQues = LatestQues.Where(m=>m.RaiseQuesTime.ToShortDateString().Equals(DateTime.Now.ToShortDateString())).ToList();
           LatestQues = LatestQues.OrderByDescending(m => m.RaiseQuesTime).Skip(pagesize * (currentIndex - 1)).Take(pagesize).ToList();
            return Json(LatestQues);
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Hotest(int currentIndex = 1)
        {
            List<QestDetail> HotestQues = new List<QestDetail>();
            HotestQues = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail VQ where ToUID='0' and VQ.Likes+CommentNum>10 order by VQ.Likes+CommentNum   desc").ToList();
            HotestQues = HotestQues.OrderByDescending(m => m.Likes).ThenBy(m=>m.RaiseQuesTime).Skip(pagesize * (currentIndex - 1)).Take(pagesize).ToList();
            return Json(HotestQues);
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult UnAns(int currentIndex = 1)
        {
            List<QestDetail> UnAnsQues = new List<QestDetail>();
            UnAnsQues = db.Database.SqlQuery<QestDetail>("select * from V_QestDetail where  ToUID='0' and CommentNum=0").ToList();
            UnAnsQues = UnAnsQues.OrderByDescending(m => m.Likes).ThenBy(m => m.RaiseQuesTime).Skip(pagesize * (currentIndex - 1)).Take(pagesize).ToList();
            return Json(UnAnsQues);
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult BestQuesAns()
        {
            List<QestDetail> BestQues = new List<QestDetail>();
            BestQues = db.Database.SqlQuery<QestDetail>("select top 10 * from V_QestDetail where ToUID != '0' and Pub=1 order by RaiseQuesTime desc").ToList();
            BestQues = BestQues.OrderByDescending(m => m.Likes).ToList();
            return Json(BestQues);
        }
        [AllowAnonymous]
        [HttpPost] 
        public JsonResult RelativeQues(string QID)
        {
            //int a = getCommonStrLen("abcdef", "acbcqdtdh5f");
            //int b = getCommonStrLen("见天我去看家了", "发发给对我方方家法官方");
            if (!string.IsNullOrEmpty(QID))
            {
                List<QBrif> AllQ = new List<QBrif>();
                Question q = db.Questions.Find(QID);
                if (q != null)
                {
                    AllQ = db.Questions.Where(m=>m.QID!=q.QID).ToList().Select(m =>
                        new QBrif()
                        {
                            QID = m.QID,
                            Title = m.Title,
                            Likes = m.Likes,
                            DegreeOfSimilarity = getCommonStrLen(q.Title,m.Title)
                        }
                    ).Where(m=>m.DegreeOfSimilarity>2).OrderByDescending(m=>m.DegreeOfSimilarity).ThenByDescending(m=>m.Likes).
                    Skip(0).Take(6).
                    ToList();
                }
                return Json(AllQ);
            }
            return null;
        }

        public int getCommonStrLen(string strA, string strB)
        {
            int a_len = strA.Length;
            int b_len = strB.Length;
            int[,] L = new int[a_len + 1, b_len + 1];
            for (int i = 1; i <= a_len; i++)
            {
                for (int j = 1; j <= b_len; j++)
                {
                    L[i,j] = strA[i - 1] == strB[j - 1]?L[i - 1,j - 1] + 1: Math.Max(L[i-1,j],L[i,j-1]);
                }
            }
            return L[a_len, b_len];
        }
        #endregion

        #region 发表、私信、评论、追问 删除问题，评论，追问

        //发表提问
        [HttpPost]
        [ValidateInput(false)]
        [LoginAuthorize]
        public JsonResult Post(string Title, string Description)
        { 
            string str = "logined|";
            if (string.IsNullOrEmpty(Title)) return Json(str+"标题必填！");
            if (string.IsNullOrEmpty(Description)) return Json(str+"请尽可能详细的描述您的问题！");
            if (Title.Length > 200) return Json(str + "标题过长");
            if (Description.Length >5000) return Json(str + "描述过长");
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
        
        [HttpPost]
        [ValidateInput(false)]
        [LoginAuthorize]
        public JsonResult Comment(string QID, string ansDescription)
        {
            string str = "logined|";
            Question Ques = db.Questions.Find(QID);
            if (Ques != null)
            {
                if (string.IsNullOrEmpty(ansDescription)) return Json(str + "评论内容不能为空！");
                if (ansDescription.Length > 5000) return Json(str + "评论内容过长");
                Ques.CommentNum++;
                TryUpdateModel(Ques);
                db.SaveChanges();
                Answer ans = new Answer()
                {
                    FromUID = User.Identity.GetUserId(),
                    ToUID = Ques.FromUID,
                    PQID = Ques.QID,
                    Description = ansDescription,
                 };
                db.Answers.Add(ans);
                db.SaveChanges();
                return Json(str+"评论成功！");
            }
            return Json(str+"评论问题不存在！");
        }
        //追问         
        [HttpPost]        
        [LoginAuthorize]     
        [ValidateInput(false)]
        public JsonResult Reply(string AID,string reDescription)
        {
            string str = "logined|";
            Answer ansCom = db.Answers.Find(AID);
            if (ansCom != null)
            {
                if (string.IsNullOrEmpty(reDescription)) return Json(str + "回复的内容不能为空！");
                if (reDescription.Length > 5000) return Json(str + "回复内容过长");
                Answer reply = new Answer() {
                    FromUID = User.Identity.GetUserId(),
                    ToUID = ansCom.FromUID,
                    FAID = ansCom.AID,
                    PQID = ansCom.PQID,
                    Description = reDescription,
                };
                ansCom.CommentNum++;
                TryUpdateModel(ansCom);
                db.Answers.Add(reply);
                db.SaveChanges();
                return Json(str + "回复成功");
            }
            return Json(str + "回复的问题不存在！");
        }
        [Authorize(Roles = "管理员")]
        [HttpPost]
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

        [Authorize(Roles = "管理员")]
        [HttpPost]
        public JsonResult DeleteAnswer(string AID)
        {
            bool flag = false;
            Answer ans = db.Answers.Find(AID);
            if (ans != null)
            {
                if (ans.FAID.Equals("0")) //删除该评论下的所有追问
                {
                    List<Answer> ansons = db.Answers.Where(m => m.FAID.Equals(ans.AID)).ToList();
                    db.Answers.RemoveRange(ansons);
                    flag = true;  //删除评论
                    string QID = ans.PQID;
                    Question ques = db.Questions.Find(QID);
                    if (QID != null)
                    {
                        ques.CommentNum--;
                        TryUpdateModel(ques);
                    }
                }
                db.Answers.Remove(ans);   //删除该追问
                db.SaveChanges();
                return Json("success|" + (flag==true?"1":"0"));
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

        //ajax为回答点赞
        [HttpPost]
        [LoginAuthorize]
        public JsonResult AddAnsLike(string AID)
        {
            Answer ans = db.Answers.Find(AID);
            string userId = User.Identity.GetUserId();
            if (ans != null)
            {
                int count = db.LikeOnce.Where(m => m.BID == AID && m.FromUID == userId).Count();
                if (count == 0)
                {
                    //增加点赞记录
                    db.LikeOnce.Add(new likeOnce() { BID = AID, FromUID = userId });
                    ans.Likes += 1;
                    TryUpdateModel(ans);
                    db.SaveChanges();
                    return Json("addsuccess");
                }
                else if (count == 1)
                {
                    likeOnce lo = db.LikeOnce.Where(m => m.BID == AID && m.FromUID == userId).First();
                    //增加点赞记录
                    db.LikeOnce.Remove(lo);
                    ans.Likes -= 1;
                    TryUpdateModel(ans);
                    db.SaveChanges();
                    return Json("delsuccess");
                }
            }
            return Json("error");
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
        public JsonResult ThisWeek(int k)
        {
            DateTime dt = DateTime.Now.AddDays(k * (-1*statisPastDays));
            List<string> week = getpast7days(dt);
            int[] quesN = getPast7QuesN(dt);
            int[] ansTN = getPast7Anses("教师", dt);
            int[] ansSN = getPast7Anses("学生", dt);
            JsonResult result = new JsonResult
            {
                Data = new
                {
                    Ques = quesN,
                    AnsTN = ansTN,
                    AnsSN = ansSN, 
                    Week = week
                }
            };
            return result;
        }
        //  double[] ansTRate = getAnsRate(ansTN,quesN);
        //double[] ansSRate = getAnsRate(ansSN, quesN);
        //AnsTRate = ansTRate,
        //AnsSRate = ansSRate,
        private double[] getAnsRate(int[] ansN, int[] quesN)
        {
            double[] rate = new double[statisPastDays];
            for (int i = 0; i < ansN.Length; i++)
            {
                rate[i] = quesN[i] == 0 ? 0 : (double)ansN[i] / quesN[i];
            }
            return rate;
        }

        public int[] getPast7Anses(string role,DateTime dt)
        {
            int[] ansN = new int[statisPastDays];
            for (int i = 0; i < statisPastDays; i++)
            {
                List<Answer> anses = db.Answers.ToList().Where(m => m.AnswerQuesTime.ToShortDateString().Equals(dt.ToShortDateString())).ToList();
                anses = anses.Where(m => UserManager.IsInRole(m.FromUID, role)).ToList();
                anses = anses.DistinctBy(m => m.PQID).ToList();
                ansN[statisPastDays - i - 1] = anses.Count();
                dt = dt.AddDays(-1);
            }
            return ansN;
        }

        public int[] getPast7QuesN(DateTime dt)
        {
            int[] quesN = new int[statisPastDays];
            for (int i = 0; i < statisPastDays; i++)
            {
                List<Question> queses = db.Questions.ToList().Where(m => m.RaiseQuesTime.ToShortDateString().Equals(dt.ToShortDateString())).ToList();
                quesN[statisPastDays - i-1] = queses.Count();
                dt = dt.AddDays(-1);
            }
            return quesN;
        }

        public List<string> getpast7days(DateTime dt)
        {
            List<string> week = new List<string>();
            for (int i = 0; i < statisPastDays; i++)
            {
                week.Add(dt.ToString("M/d"));
                dt = dt.AddDays(-1);
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