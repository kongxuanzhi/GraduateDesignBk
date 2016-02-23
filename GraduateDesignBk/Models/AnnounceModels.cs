using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public class Announce
    {
        public Announce()
        {
            ANID = Guid.NewGuid().ToString();
            Time = DateTime.Now;
            ReadTimes = 0;
            Prop = 0;
        }
        [Key]
        [StringLength(120)]
        public string ANID { get; set; }
        [Display(Name = "公告标题")]
        public string Title { get; set; }
        [Display(Name = "公告内容")]
        public string Content { get; set; }
        [StringLength(120)]
        public string FromUID { get; set; }
        public DateTime Time { get; set; }
        public int Prop { get; set; }  //优先级  2 置顶 1 最新 0 一般
        public int ReadTimes { get; set; }
    }
}
