using FenXiao.Model;
using FenXiao.Web.Areas.Admin.Models;
using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class AWeChatController : AdminControllerBase
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

        // GET: Admin/AWeChat
        //所有营销页(共有+私有)
        public ActionResult AllPage()
        {
            return View();
        }
        public ActionResult GetAllPage(int? secho)
        {
            var query = db.Pro2Page.Select(a => new Pagedatamodel
            {
                Id = a.Id,
                ProductName = a.Product.Name,
                PageName = a.Page.Name,
                CompanyName = a.Company.CompanyName,
                State = a.Product.State == (int)EnumPageState.publicState ? "公开" : "私有",
                PageId = a.PageId
            });
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

        //线路列表  点击可查看对应的营销页
        public ActionResult LineList()
        {
            return View();
        }
        public ActionResult GetLineList(int? secho)
        {
            var query = db.Products.OrderByDescending(a => a.Id).Select(a => new { a.Id, a.Name,a.FuJian,a.State });
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

        //点击每条线路, 打开对应的营销页列表  id为线路ID
        public ActionResult PageManage(int id)
        {
            ViewBag.lineId = id;
            return View();
        }
        public ActionResult GetPageManage(int lineId,int? secho)
        {
            var query = db.Pro2Page.Where(a => a.ProductId == lineId).OrderByDescending(a => a.Id).Select(a => new Pagedatamodel2()
            {
                Id = a.Id,
                PageName = a.Page.Name,
                State = a.Product.State == (int)EnumPageState.publicState ? "公开" : "私有",
                PageId = a.PageId,
                TempId = a.PageId,
                TempId2 = a.PageId
            });
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
        
        //删除营销页面
        public ActionResult DeletePage(Guid id)
        {
            var page = db.Pages.FirstOrDefault(a => a.Id == id);
            if (page == null)
            {
                return HttpNotFound();
            }
            db.Pages.Remove(page);
            var relation = db.Pro2Page.FirstOrDefault(a=>a.PageId == id);
            var idrecord = relation.ProductId;
            if (relation != null)
            {
                db.Pro2Page.Remove(relation);
            }
            db.SaveChanges();
            return RedirectToAction("PageManage", new { lineId = idrecord });
        }

        //预览页
        public ActionResult YuLan(Guid id)
        {
            var page = db.Pages.FirstOrDefault(a => a.Id == id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        //添加和编辑  id为线路Id
        public ActionResult AddPage(int id)
        {
            //..
            return View();
        }
        [HttpPost]
        public ActionResult AddPage(Page newPage)
        {
            //..
            return View(); 
        }
    }
}