using FenXiao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Areas.Admin.Models
{
    public class IndexModel
    {
        public IndexModel() {
            NewApplyList = new List<Apply>();
            NewLineList = new List<Product>();
        }
        public int NewApplyNum { get; set; }
        public int NewLineNum { get; set; }
        public List<Apply> NewApplyList { get; set; }
        public List<Product> NewLineList { get; set; }
    }
}