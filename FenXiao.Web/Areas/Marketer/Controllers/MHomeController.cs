using FenXiao.Model;
using FenXiao.Web.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FenXiao.Web.Models;
using Webdiyer.WebControls.Mvc;
namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MHomeController : MarketerControllerBase
    {

        public ActionResult Help()
        {
            List<HelpModel> hms = new List<HelpModel>();
            var list = db.HelpTypes.OrderByDescending(a => a.Weight).ToList();
            foreach (var item in list)
            {
                HelpModel hm = new HelpModel() { };
                hm.Name = item.Name;
                hm.HelpArticles = item.HelpArticles.OrderByDescending(a => a.Weight).ToList();
                hms.Add(hm);
            }
            return View(hms);
        }

        public ActionResult HelpContent(int id)
        {
            var ha = db.HelpArticles.Find(id);
            if (ha == null)
            {
                return HttpNotFound();
            }
            return View(ha);
        }
    }
}