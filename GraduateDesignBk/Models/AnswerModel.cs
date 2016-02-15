using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraduateDesignBk.Models
{
    public class Answer //发表的帖子
    {
        public Answer()
        {
            AID = Guid.NewGuid().ToString();
            AnswerQuesTime = DateTime.Now;
            Likes = 0;
            FAID = "0";
        }
        [Key]
        [StringLength(128)]
        public string AID { get; set; }

        [StringLength(108)]
        public string FromUID { get; set; }

        [DefaultValue("0")]
        [StringLength(128)]
        public string ToUID { get; set; }

        [DefaultValue("0")]
        [StringLength(128)]
        public string FAID { get; set; }  //父级回答

        [DefaultValue("0")]
        [StringLength(128)]
        public string PQID { get; set; }    //外键关联QuestionModel 表的主键

        public DateTime AnswerQuesTime { get; set; }   //问题提出时间 出发回答问题时间 排序列出
        
        [StringLength(500)]
        public string Description { get; set; }  //内容

        public int Likes { get; set; }   // 赞
    }
}