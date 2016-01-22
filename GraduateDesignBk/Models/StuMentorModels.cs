using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public class StuMentor
    {
        public StuMentor()
        {
            SMID = Guid.NewGuid().ToString();
        }
        [Key]
        [StringLength(128)]
        public string SMID { get; set; }

        [StringLength(128)]
        public string StudentUID { get; set; }
        [ForeignKey("StudentUID")]
        public ApplicationUser User2 { get; set; }

        [StringLength(128)]
        public string TeacherUID { get; set; }

        [ForeignKey("TeacherUID")]
        public ApplicationUser User4 { get; set; }
    }
}
