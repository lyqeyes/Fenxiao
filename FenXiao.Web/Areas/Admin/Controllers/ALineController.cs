using FenXiao.Model;
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
        public override int PageSize
        {
            get
            {
                return 15;
            }
        }
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

        

        #region 线路部分
        //1.首页
        public ActionResult AllLines(DateTime? sst, DateTime? set,
           DateTime? cst, DateTime? cet, int luxianid = 0,
            int resetnum = -1, string search = "", int id = 1, int pageid = 1,int companyid = -1)
        {
            if (Request.IsAjaxRequest())
            {
                if (resetnum != -1)  //即有赋值
                {
                    return View("_AllLinesPartial", ConditionSearchPartial(sst, set, cst, cet, luxianid, resetnum, id, companyid));
                    //return MySelledLuxianPartial(sst, set, cst, cet, luxianid, restnum, pageid);
                }
                else
                {
                    return View("_AllLinesPartial", ContentSearchPartial(search, id));
                }
            }
            else
            {
                var lines = db.Products.OrderByDescending(a => a.CreateTime);
                return View(lines.ToPagedList(id, PageSize));
            }
        }

        //2.全文检索
        public PagedList<Product> ContentSearchPartial(string search = "", int id = 1)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var v = Searcher.Search(search);
                var Products = db.Products.Where(a => a.User.CompanyId == LoginInfo.CompanyId &&
                a.SendGroupTime < DateTime.Now && a.State == (int)EnumProduct.zhengchang && v.Contains(a.Id)).
                OrderByDescending(a => a.Id).ToPagedList(id, this.PageSize);
                return Products;
            }
            else
            {
                var Products = db.Products.Where(a => a.User.CompanyId == LoginInfo.CompanyId &&
                a.SendGroupTime < DateTime.Now && a.State == (int)EnumProduct.zhengchang).OrderByDescending(a => a.Id).ToPagedList(id, this.PageSize);
                return Products;
            }
        }

        //3.条件检索
        public PagedList<Product> ConditionSearchPartial(DateTime? sst, DateTime? set,
           DateTime? cst, DateTime? cet, int luxianid = 0, int restnum = -1, int id = 1,int companyid = -1)
        {
            var pro = db.Products as IQueryable<Product>;

            if (sst.HasValue)
            {
                pro = pro.Where(a => a.SendGroupTime > sst.Value);

            }
            if (set.HasValue)
            {
                pro = pro.Where(a => a.SendGroupTime < set);
            }
            if (cst.HasValue)
            {
                pro = pro.Where(a => a.CreateTime > cst);

            }
            if (cet.HasValue)
            {
                pro = pro.Where(a => a.CreateTime < cet);

            }
            if (luxianid > 0)
            {
                pro = pro.Where(a => a.Id == luxianid);

            }
            if (restnum == 1)
            {
                pro = pro.Where(a => a.IsHot == 1);

            }
            if (restnum == 2)
            {
                pro = pro.Where(a => a.IsHot == 0);
            }
            if (companyid > -1)
            {
                pro = pro.Where(a => a.User.CompanyId == companyid);
            }
            return (pro.OrderByDescending(a => a.Id).ToPagedList(id, PageSize));
        }
        //4. 每一条线路的出售情况查看<所有客户,公司, 退订单..>
        //4.1所有客户
        public ActionResult Line_AllCustomer(int productId,int id = 1)
        {
            ViewBag.productId = productId;
            var list = db.CustomerInfoes.Where(a => a.ChildProduct.ProductId == productId);
            return View(list.OrderByDescending(a => a.CreateTime).ToPagedList(id,PageSize));
        }
        //4.2所有公司
        public ActionResult Line_AllCompany(int productId, int id = 1)
        {
            return View();
        }
        //4.3所有订单
        public ActionResult Line_AllOrder(int productId, int id = 1)
        {
            return View();
        }
        //4.4所有退单
        public ActionResult Line_AllReturnOrder(int productId, int id = 1)
        {
            return View();
        }

        //获取公司列表
        public ActionResult LineSearchByRangeParialView()
        {
            var temp = ((int)EnumCompany.pifa).ToString();
            var list = db.Companies.Where(a => a.CompanyRole.Contains(temp) && a.State == 1).ToList();
            return PartialView(list);
        }

        //线路介绍页
        public ActionResult LineDetail(int id)
        {
            return View(db.Products.FirstOrDefault(a => a.Id == id));
        }
        #endregion

        //禁用等操作
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

        //订单
        public ActionResult LineSaleSituation(int? id, int productId)
        {
            var sales = db.OrderForms.Where(a => a.ProductId == productId).OrderByDescending(a => a.CreateTime);
            return View(sales.ToPagedList(id??1,25));
        }
        
        //退单
        public ActionResult LineReturnOrder(int? id, int productId)
        {
            var sales = db.ReturnForms.Where(a => a.ProductId == productId).OrderByDescending(a => a.CreateTime);
            return View(sales.ToPagedList(id ?? 1, 25));
        }
        //退单详情
        public ActionResult LineReturnOrderDetail(int id)
        {
            //1.获取退单实例
            var returnorder = db.ReturnForms.FirstOrDefault(a => a.Id == id);
            if (returnorder == null)
            {
                return HttpNotFound();
            }
            else
            {
                
                return View();
            }
        }
	}
}