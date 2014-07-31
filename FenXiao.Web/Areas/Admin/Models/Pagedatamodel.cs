using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Areas.Admin.Models
{
    public class Pagedatamodel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PageName { get; set; }
        public string CompanyName { get; set; }
        public string State { get; set; }
        public Guid PageId { get; set; }
    }
}