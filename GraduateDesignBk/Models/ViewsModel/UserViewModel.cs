using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public class SearchAndPage
    {
        public SearchAndPage()
        {
            CurIndex = 1;
            PageNum = 1;
        }
        public string userType { get; set; }
        public string SearchName { get; set; }  //一卡通号
        public string SerachRealName { get; set; }  //姓名
        public Level SearchLevel { get; set; }
        public Mayjor SearchMayor { get; set; }
        public string type { get; set; }
        public Size PageSize { get; set; }  //每页条数
        public int CurIndex { get; set; }  //当前页
        public int TotalCount { get; set; }  //一共多少条
        public int PageNum { get; set; }  //页数

        public List<UserViewModel> UserItems { get; set; }
    }
    public enum Size
    {
        每页8条 =  0,
        每页12条 = 4,
        每页16条 = 8,
        每页20条 = 12
    }

    public class UserViewModel  
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Photo { get; set; }
        public Mayjor mayjor { get; set; }
        public Level level { get; set; }
        public string Comment { get; set; }
    }

    public  class AddNewUserModel
    {
        [Required(ErrorMessage = "一卡通号必填")]
        [Display(Name = "一卡通号：")]
        [MinLength(10,ErrorMessage = "必须为10位数字"),MaxLength(10,ErrorMessage = "必须为10位数字")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "姓名必填")]
        [Display(Name = "姓名：")]
        public string RealName { get; set; }   //一卡通号
        [Range(1,double.MaxValue,ErrorMessage = "专业必选")]
        [Display(Name = "专业：")]
        public Mayjor mayjor { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "学级必选")]
        [Display(Name = "级别：")]
        public Level level { get; set; }
        [StringLength(490, ErrorMessage = "内容不能超过400字")]
        [Display(Name = "评语：")]
        public string Comment { get; set; }
        public string RoleType { get; set; }
        //注 ：邮箱和电话由用户自己完善 //还有头像，密码设置为初始密码 为一卡通号
    }

    public class UserDetailModel
    {
        public PersonInfo personInfo { get; set; }
        public List<Bar> bars { get; set; }
        //上传文件
        public List<File> files { get; set; }
        public List<Notice> msgs { get; set; }

        public List<UserViewModel> stuOrTeaches { get; set; }
    }

    public class PersonInfo
    {
        public string Id { get; set; }

        [Display(Name = "一卡通号：")]
        public string UserName { get; set; }
        [Display(Name = "真实姓名：")]
        public string RealName { get; set; }
        public string Photo { get; set; }

        [Display(Name = "邮箱：")]
        public string Email { get; set; }
        [Display(Name = "电话号码：")]

        public string PhoneNumber { get; set; }

        [Display(Name = "专业：")]
        public Mayjor mayjor { get; set; }

        [Display(Name = "学级：")]
        public Level level { get; set; }
        [Display(Name ="评语：")]
        public string Comment { get; set; }

        [Display(Name ="身份：")]
        public string userType { get; set; }
    }

    public class BarDetail
    {
        public string BID { get; set; }
        public string FromUID { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string FromPhoto { get; set; }
        public string ToUID { get; set; }
        public string FBID { get; set; }  //父级问题或回答
        public string PBID { get; set; }    //祖父级问题或回答
        public bool Pub { get; set; }  //是否公开
        public DateTime RaiseQuesTime { get; set; }   //问题提出时间 出发回答问题时间 排序列出
        public string Content { get; set; }  //内容
    }

    public class PersonBars
    {
        public List<BarDetail> pbars { get; set; }
        public List<BarDetail> fbars { get; set; }
        public List<BarDetail> sbars { get; set; }
        public string Id { get; set; }
        public string userType { get; set; }
    }

}
