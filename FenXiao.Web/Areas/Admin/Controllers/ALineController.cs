using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class ALineController : AdminControllerBase
    {
        //
        // GET: /Admin/ALine/
        public ActionResult AllLines(int? id)
        {
            var lines = db.Products.OrderByDescending(a => a.CreateTime);
            return View(lines.ToPagedList(id??1,25));
        }

        public ActionResult LineSearch()
        {
            //..检索
            return View("AllLines");
        }

        public ActionResult LineDetail(int id)
        {
            return View(db.Products.FirstOrDefault(a=>a.Id == id));
        }

        //线路的订单
        public ActionResult LineSaleSituation(int? id, int productId)
        {
            var sales = db.OrderForms.Where(a => a.ProductId == productId).OrderByDescending(a => a.CreateTime);
            return View(sales.ToPagedList(id??1,25));
        }
        public ActionResult LineReturnOrder(int? id, int productId)
        {
            var sales = db.ReturnForms.Where(a => a.ProductId == productId).OrderByDescending(a => a.CreateTime);
            return View(sales.ToPagedList(id ?? 1, 25));
        }
        public ActionResult LineChangeState(int id)
        {
            var line = db.Products.FirstOrDefault(a => a.Id == id);
            if (line != null)
            {
                line.State = line.State == 1 ? 0 : 1;
                db.Entry(line).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("AllLines");
        }

	}
}