using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FenXiao.Web.Areas.Wholesaler.Controllers
{
    public class WOrderController : WholesalerControllerBase
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}