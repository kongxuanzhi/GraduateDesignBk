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
    public class Notice
    {
        public Notice()
        {
            this.CreateTime = DateTime.Now;
            NID = Guid.NewGuid().ToString();
        }
        [Key]
        [StringLength(128)]
        public string NID { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(500)]
        public string Detail { get; set; }

        [StringLength(128)]
        public string FromUID { get; set; }
        [ForeignKey("FromUID")]
        public ApplicationUser User2 { get; set; }


        [DefaultValue(false)]
        public bool Readed { get; set; }
    }
}
