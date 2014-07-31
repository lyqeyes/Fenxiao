using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Areas.Admin.Models
{
    public class Pagedatamodel2
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string State { get; set; }
        public Guid PageId { get; set; }
        public Guid TempId { get; set; }
        public Guid TempId2 { get; set; }
    }
}