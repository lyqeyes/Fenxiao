using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FenXiao.Model;
namespace FenXiao.Web.Areas.Admin.Models
{
    public class ReturnOrderDetailModel
    {
        public ReturnOrderDetailModel() {
            cuslist = new List<CustomerInfo>();
        }
        public ReturnForm returnform {get;set;}
        public List<CustomerInfo> cuslist {get;set;}
        public int productId { get; set; }
    }
}