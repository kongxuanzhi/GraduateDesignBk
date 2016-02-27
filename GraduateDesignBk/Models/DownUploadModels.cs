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
    public class DownUpload
    {
        public DownUpload()
        {
            Time = DateTime.Now;
            DID = Guid.NewGuid().ToString();
        }
        [Key]
        [StringLength(128)]
        public string DID  { get; set; }  //主键

        public DateTime Time { get; set; }

        [StringLength(128)]
        public string ToUID { get; set; }    //接收文件的拥有着   //外键
        [ForeignKey("ToUID")]
        public ApplicationUser user { get; set; }   //外键

        [StringLength(128)]
        public string FID { get; set; }       //文件的Id

        [ForeignKey("FID")]
        public File Files { get; set; }  //外键 到FileModel表

        public ReadState Readstate { get; set; }      //是否接收文件
    }

    public enum ReadState
    {
        未读,
        已读
    }

}   
