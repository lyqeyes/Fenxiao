using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MErrorController : Controller
    {

        public new ActionResult HttpNotFound()
        {
            return View();
        }

        public ActionResult BadRequestt()
        {
            return View();
        }

        public ActionResult NoPermission()
        {
            return View();
        }

        public ActionResult HandleByOther(string url)
        {
            ViewBag.Url = url;
            return View();
        }
    }
}