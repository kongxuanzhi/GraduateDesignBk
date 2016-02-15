using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models.ViewsModel
{
    public class MsgViewModel
    {
        public string NID { get; set; }
        [Display(Name ="发送时间")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "信息内容")]
        public string  Detail { get; set; }
        [Display(Name = "发送人")]
        public string  FromUID { get; set; }
        public string  FromId { get; set; }
        [Display(Name = "标题")]
        public string Title { get; set; }
    }

}
