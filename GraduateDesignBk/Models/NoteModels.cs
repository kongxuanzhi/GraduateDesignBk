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
        public Note()
        {
            NTID = Guid.NewGuid().ToString();
            Time = DateTime.Now;
        }
        [Key]
        [StringLength(120)]
        public string NTID { get; set; }
        [Display(Name ="通知内容")]
        public string Content { get; set; }
        [StringLength(120)]
        public string FromUID { get; set; }
        public DateTime Time { get; set; }
    }
}
