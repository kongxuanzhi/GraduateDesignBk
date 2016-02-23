using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{

    public class AnnouandP
    {
        public AnnouandP()
        {
            page = new Paging();
            AnnouItems = new List<AnnounceView>();
        }
        public string STitle { get; set; }
        public string SuserName { get; set; }

        public Paging page { get; set; }
        public List<AnnounceView> AnnouItems { get; set; }
    }

    public class AnnounceView
    {
        public string ANID { get; set; }
        [Display(Name = "公告标题")]
        public string Title { get; set; }
        public string Content { get; set; }
        [StringLength(120)]
        public string FromUID { get; set; }
        public string FromName { get; set; }
        public string Time{ get; set; }
        public int Prop { get; set; }  //优先级  2 置顶 1 最新 0 一般
        public int ReadTimes { get; set; }
    }
    public class PostAnnounceView
    {
        public string ANID { get; set; }
        [Display(Name = "公告标题")]
        [StringLength(100,ErrorMessage ="公告标题过长，100字以内")]
        [Required(ErrorMessage = "公告标题必填")]
        public string Title { get; set; }
        [Display(Name = "公告内容")]
        [Required(ErrorMessage = "公告内容必填")]
        public string Content { get; set; }
    }
}
