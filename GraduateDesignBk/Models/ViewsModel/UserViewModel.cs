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
        public string userType { get; set; }
        public string SearchName { get; set; }
        public string SerachRealName { get; set; }
        public string SearchLevel { get; set; }
        public string SearchMayor { get; set; }
        public int PageSize { get; set; }  //每页条数
        public int CurIndex { get; set; }  //当前页
        public int TotalCount { get; set; }  //一共多少条
        public int PageNum { get; set; }  //页数

        public List<UserViewModel> UserItems { get; set; }
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
        [Display(Name = "专业：")]
        public Mayjor mayjor { get; set; }
        [Display(Name = "级别：")]
        public Level level { get; set; }
        [StringLength(490, ErrorMessage = "内容不能超过400字")]
        [Display(Name = "评语：")]
        public string Comment { get; set; }
        public string RoleType { get; set; }
        //注 ：邮箱和电话由用户自己完善 //还有头像，密码设置为初始密码 为一卡通号
    }
}
