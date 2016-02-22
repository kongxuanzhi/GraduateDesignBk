using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public class CreateNoteView
    {
   
        [Display(Name = "通知内容")]
        [StringLength(120,ErrorMessage ="通知请在120字之间")]
        public string Content { get; set; }
        [StringLength(120)]
        public string FromUID { get; set; }
        public DateTime Time { get; set; }
    }
}
