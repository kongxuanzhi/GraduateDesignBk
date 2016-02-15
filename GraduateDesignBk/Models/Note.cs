using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public class Note
    {
        [Key]
        [StringLength(120)]
        public string NTID { get; set; }
        [StringLength(200)]
        [Display(Name ="公告内容：")]
        public string Content { get; set; }
        [StringLength(120)]
        public string FromUID { get; set; }
        public DateTime Time { get; set; }
    }
}
