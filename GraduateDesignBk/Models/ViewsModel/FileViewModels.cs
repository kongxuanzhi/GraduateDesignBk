using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{


    public class FileSandP
    {
        public FileSandP()
        {
            page = new Paging();
            Spub = true;
            FileItems = new List<FileViewModel>();
        }

        public string Sname { get; set; }
        public bool Spub { get; set; }
        public string Stype { get; set; }
        public string  SuserName { get; set; }

        public Paging page { get; set; }
        public List<FileViewModel> FileItems { get; set; }
    }

    public class FileViewModel
    {
        public string FID { get; set; }
        [Display(Name ="文件名")]
        public string Name { get; set; }
        [Display(Name = "发件人")]
        public string FromUID  { get; set; }  //上传人姓名
        public string FromId { get; set; }  //上传人Id
        [Display(Name = "公开不")]
        public bool  Pub{ get; set; }
        [Display(Name = "发送时间")]
        public string UploadTime { get; set; }
        [Display(Name = "类型")]
        public string Type { get; set; }
        [Display(Name = "大小")]
        public string Size { get; set; }
        public int DownloadTimes { get; set; }

    }

    public class FileDetail
    {
        public FileViewModel file {get; set; }
        public List<ApplicationUser> Receives { get; set; }
    }
   
}
  