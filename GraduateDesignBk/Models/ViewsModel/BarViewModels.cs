using System;
using System.Collections.Generic;

namespace GraduateDesignBk.Models
{
    public class QestDetail
    {
        public string QID { get; set; }
        public string FromUID { get; set; }  //编号
        public string userType { get; set; }
        public string FromName { get; set; }  //  姓名
        public string ToName { get; set; }   //老师姓名
        public string FromPhoto { get; set; } //头像 和用户表关联
        public string ToUID { get; set; }   // 编号 为空时就是公开提问，否则为私信 指向私信的老师
        public bool Pub { get; set; }  //是否公开
        public DateTime RaiseQuesTime { get; set; }   //问题提出时间 出发回答问题时间 排序列出
        public string Title { get; set; }
        public string Description { get; set; }  //内容
        public int Likes { get; set; }
        public bool Solved { get; set; } 
        public int CommentNum { get; set; }
    }

    public class AnswerDetail
    {
        public string AID { get; set; }
        public string FromUID { get; set; }  //编号 
        public string ToUID { get; set; }  //名称
        public string userType { get; set; }
        public string FromName { get; set; }  // 姓名
        public string ToName{ get; set; }   //编号
        public string FromPhoto { get; set; } //头像
        public string FAID { get; set; }  //父级回答   为空时代表是二级回答
        public string PQID { get; set; }    //某个问题下的回答
        public DateTime AnswerQuesTime { get; set; }   //问题提出时间 出发回答问题时间 排序列出
        public string Description { get; set; }  //内容
        public int Likes { get; set; }
    }

    public class BarViewModel
    {
        public BarViewModel()
        {
           Bars = new BarDetailModel();
           Bars.pbars = new List<QestDetail>();
           Bars.fbars = new List<AnswerDetail>();
           Bars.sbars = new List<AnswerDetail>();

            page = new Paging();
        }
        public string SAuthor { get; set; }
        public string SQue { get; set; }
        public IsPub isPub { get; set; }

        public Paging page { get; set; }
        public BarDetailModel Bars { get; set; }
    }

    public enum IsPub
    {
        公开 = 1,
        私密
    }

    public class BarDetailModel
    {
        public List<QestDetail> pbars { get; set; }
        public List<AnswerDetail> fbars { get; set; }
        public List<AnswerDetail> sbars { get; set; }
    }
}