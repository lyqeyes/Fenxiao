using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Areas.Admin.Models
{
    public class Companydatamodel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRole { get; set; }
        public string City { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public int State { get; set; }
    }
}