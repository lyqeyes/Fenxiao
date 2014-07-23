using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class AHomeController : AdminControllerBase
    {
        // GET: Admin/AHome
        public ActionResult Index()
        {
            return View();
        }
    }
}