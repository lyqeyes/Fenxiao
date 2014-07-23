using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FenXiao.Model;

namespace FenXiao.Web.Areas.Marketer.Models
{
    public class LineSearchModel
    {
        public List<Product> res {get;set;}
        public List<int> Choose { get; set; }
        public string type { get; set; }
    }
}