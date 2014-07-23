using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FenXiao.Model;

namespace FenXiao.Web.Areas.Marketer.Models
{
    public class LineTypeModel
    {
        public List<IGrouping<string,LuXianType>> LuXianTypes { get; set; }
        public List<int> Choose { get; set; }

        public string type { get; set; }
    }
}