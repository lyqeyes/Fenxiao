using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FenXiao.Model;

namespace FenXiao.Web.Areas.Admin.Models
{
    public class InfoCheckModel
    {
        public int TempCompanyId { get; set; }
        public TempCompany CompanyNew { get; set; }
        public Company CompanyOld { get; set; }
    }
}