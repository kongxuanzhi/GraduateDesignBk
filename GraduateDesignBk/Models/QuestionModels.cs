using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraduateDesignBk.Models
{
    public class Question  //发表的帖子
    {
        public Question()
        {
            QID = Guid.NewGuid().ToString();
            RaiseQuesTime = DateTime.Now;
            Likes = 0;
            Solved = false;
            CommentNum = 0;
        }
        [Key]
        [StringLength(128)]
        public string QID { get; set; }

        [StringLength(108)]
        public string FromUID { get; set; }

        [DefaultValue("0")]
        [StringLength(128)]
        public string ToUID { get; set; }
        
        [DefaultValue(false)]
        public bool Pub { get; set; }  //是否公开

        public DateTime RaiseQuesTime { get; set; }   //问题提出时间 出发回答问题时间 排序列出

        [StringLength(200)]
        public string Title { get; set; }  //标题

        [StringLength(500)]
        public string  Description { get; set; }  //内容

        public int Likes { get; set; }   // 赞

        [DefaultValue(false)]
        public bool Solved { get; set; }  //解决没

        public int CommentNum { get; set; }  //评论数 触发器
    }
}
