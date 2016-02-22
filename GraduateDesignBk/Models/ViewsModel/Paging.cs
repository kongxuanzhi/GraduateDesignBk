using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateDesignBk.Models
{
    public class Paging
    {
        public Paging()
        {
            CurIndex = 1;
            PageNum = 1;
        }
        public int FilePageSize { get; set; }
        public pageSize PageSize { get; set; }  //每页条数
        public int CurIndex { get; set; }  //当前页
        public int TotalCount { get; set; }  //一共多少条
        public int PageNum { get; set; }  //页数
    }

    public enum pageSize
    {
        每页8条 = 0,
        每页12条 = 4,
        每页16条 = 8,
        每页20条 = 12,
        全部 = int.MaxValue - 8
    }
}
