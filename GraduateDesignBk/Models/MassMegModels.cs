using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public class MassMeg
    {
        public  MassMeg()
        {
            MID = Guid.NewGuid().ToString();
        }
        [Key]
        [StringLength(128)]
        public string MID { get; set; }       //表格字段ID，设置为自增

        [Required]
        [StringLength(128)]
        public string NID { get; set; }       //消息号关联T_ notice
        [ForeignKey("NID")]
        public Notice notices { get; set; }


        [StringLength(128)]
        public string ToUID { get; set; }      //接收人ID(用户表) T_user
        [ForeignKey("ToUID")]
        public ApplicationUser User2 { get; set; }
    }
}
