using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string StuNum { get; set; }
        public string  Photo { get; set; }
        public Mayjor mayjor { get; set; }
        public Level level { get; set; }
        public string Comment { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public  class AddNewUserModel
    {
        [Required(ErrorMessage="姓名必填")]
        [Display(Name = "姓名：")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "一卡通号必填")]
        [Display(Name = "一卡通号：")]
        public string StuNum { get; set; }   //一卡通号
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
