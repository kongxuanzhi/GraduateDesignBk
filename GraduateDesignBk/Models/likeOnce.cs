using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public  class likeOnce
    {
        public likeOnce(){
            LID = Guid.NewGuid().ToString();
        }
        [Key]
        public string LID { get; set; }
        public string BID { get; set; }
        public string FromUID { get; set; }
    }
}
