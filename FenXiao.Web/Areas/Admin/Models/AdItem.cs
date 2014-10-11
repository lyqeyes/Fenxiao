using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Areas.Admin.Models
{
    public class AdItem
    {
        public long Id { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string CreateUserName { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string AdContent { get; set; }
    }
}