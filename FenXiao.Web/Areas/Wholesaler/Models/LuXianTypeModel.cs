using FenXiao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Areas.Wholesaler.Models
{
    public class LuXianTypeModel
    {
        public string Key { get; set; }
        public IGrouping<string, LuXianType> groups { get; set; }
    }
}