using FenXiao.Web.Areas.Admin.Models;
using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class ALineController : AdminControllerBase
    {
        private List<string> GetPropertyList(object obj)
        {
            var propertyList = new List<string>();
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                object o = property.GetValue(obj, null);
                propertyList.Add(o == null ? "" : o.ToString());
            }
            return propertyList;
        }
        //
        // GET: /Admin/ALine/
        public ActionResult AllLines(int? id)
        {
            //var lines = db.Products.OrderByDescending(a => a.CreateTime);
            //return View(lines.ToPagedList(id??1,25));
            return View();
        }

        public ActionResult GetAllLines(int? secho)
        {
            var query_temp = db.Products.Select(a => new { a.Id, a.Name, a.State, a.Count, a.RemainCount }).ToList();
            var query = new List<Linedatamodel>();
            foreach(var item in query_temp)
            {
                if(item.State == 1 && item.RemainCount == item.Count)
                    query.Add(new Linedatamodel(){
                        Id = item.Id, 
                        Name =  item.Name, 
                        State = (int)EnumLineStates.candisable 
                    });
                else if(item.State == 1 && item.RemainCount < item.Count)
                    query.Add(new Linedatamodel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        State = (int)EnumLineStates.cannotdisable
                    });
                else
                    query.Add(new Linedatamodel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        State = (int)EnumLineStates.disable
                    });
            }
            var objs = new List<object>();
            foreach (var item in query)
            {
                objs.Add(GetPropertyList(item).ToArray());
            }
            return Json(new
            {
                sEcho = secho,
                iTotalRecords = query.Count(),
                aaData = objs,
            }, JsonRequestBehavior.AllowGet);
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