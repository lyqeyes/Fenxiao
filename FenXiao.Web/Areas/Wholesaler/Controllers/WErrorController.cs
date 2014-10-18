using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Areas.Wholesaler.Controllers
{
    public class WErrorController : Controller
    {
        [AuthorizeIgnore]
        public new ActionResult HttpNotFound(int kind=1)
        {
            return View(kind);
        }
        [AuthorizeIgnore]
        public ActionResult BadRequestt(int kind = 1)
        {
            return View(kind);
        }
        [AuthorizeIgnore]
        public ActionResult NoPermission(int kind = 1)
        {
            return View(kind);
        }
        [AuthorizeIgnore]
        public ActionResult HandleByOther(string url, int kind = 1)
        {
            ViewBag.Url = url;
            return View(kind);
        }
        [AuthorizeIgnore]
        public ActionResult LoginOutTime(int kind = 1)
        {
            return View(kind);
        }

        [AuthorizeIgnore]
        public ActionResult Shenhe()
        {
            return View();
        }

        [AuthorizeIgnore]
        public ActionResult EditShenhe()
        {
            return View();
        }
    }
}