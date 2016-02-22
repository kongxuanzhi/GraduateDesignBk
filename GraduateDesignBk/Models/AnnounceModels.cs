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
        public int ReadTimes { get; set; }
    }
}
