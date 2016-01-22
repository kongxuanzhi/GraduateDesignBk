using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public class Bar  //发表的帖子
    {
        public Bar()
        {
            BID = Guid.NewGuid().ToString();
        }
        [Key]
        [StringLength(128)]
        public string BID { get; set; }

        [StringLength(108)]
        public string FromUID { get; set; }
        [ForeignKey("FromUID")]
        public ApplicationUser User1 { get; set; }


        [DefaultValue("0")]
        [StringLength(128)]
        public string ToUID { get; set; }
        [ForeignKey("ToUID")]
        public ApplicationUser User2 { get; set; } //是向谁提问

        [DefaultValue("0")]
        [StringLength(128)]
        public string FBID { get; set; }  //父级问题或回答

        [DefaultValue("0")]
        [StringLength(128)]
        public string PBID { get; set; }    //祖父级问题或回答

        [DefaultValue(false)]
        public bool Public { get; set; }  //是否公开

        public DateTime RaiseQuesTime { get; set; }   //问题提出时间 出发回答问题时间 排序列出

        public DateTime AnswerTime { get; set; }  // 问题回答时间

        [StringLength(500)]
        public string  Content { get; set; }  //内容
    }
}
