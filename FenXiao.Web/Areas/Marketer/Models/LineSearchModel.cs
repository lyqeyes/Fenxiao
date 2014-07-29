using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FenXiao.Model;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Marketer.Models
{
    public class LineSearchModel
    {
        public PagedList<Product> res {get;set;}
        public List<int> Choose { get; set; }
        public string type { get; set; }

        public string from { get; set; }
        public string to { get; set; }
    }
}