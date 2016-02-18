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
    public class File
    {
        public File()
        {
            FID = Guid.NewGuid().ToString();
            UploadTime = DateTime.Now;
        }
        [Required]
        [StringLength(128)]
        [Key]
        public string FID { get; set; }

        [StringLength(128)]
        public string FileSeq { get; set; }

        [StringLength(120)]
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool Pub { get; set; }

        public DateTime UploadTime { get; set; }

        [StringLength(80)]
        public string Type { get; set; }

        [StringLength(80)]
        public string Size { get; set; }

        public int DownloadTimes { get; set; }

        [DefaultValue("0")]
        [StringLength(120)]
        public string FromUID { get; set; }
        [ForeignKey("FromUID")]
        public ApplicationUser User { get; set; }

        public virtual ICollection<DownUpload> Products { get; set; }
    }  
}
