using FenXiao.Model;
using FenXiao.Web.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Wholesaler.Controllers
{
    public class WWechatController : WholesalerControllerBase
    {
        public ActionResult Management(int id = 0,int pageid=0,string luxianname = "")
        {
            if (Request.IsAjaxRequest())
            {
                return View("ManagementPartial", ManagementPartial(luxianname, pageid));
            }
            else
            {
                var Products = db.Products.Where(a => a.User.CompanyId ==
               this.LoginInfo.CompanyId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                return View(Products);
            }
           
        }

        public PagedList<Product> ManagementPartial(string luxianname,int id)
        {
            var res = Searcher.Search(luxianname);
            return db.Products.Where(a => res.Contains(a.Id)).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
        }

        public ActionResult DetailList(int id)
        {
            var pro = db.Pro2Page.Where(
                a => a.CompanyId == this.LoginInfo.CompanyId && 
                    a.ProductId == id).ToList();
            return View(pro);
        }

        public ActionResult Index(int id = 0,int pageid=0, string search = "", string luxianname="", int wechatid=0)
        {
            if (Request.IsAjaxRequest())
            {
                if (string.IsNullOrEmpty(search))
                {
                    return View("MyWechatPartial", MyWechatPartial(wechatid, luxianname, id));
                }
                else
                {
                    return View("MyWechatSearchPartial", MyWechatSearchPartial(search, pageid));
                }
            }
            else
            {
                var pages = db.Pro2Page.Where(a =>
                a.Page.State == (int)EnumPageState.publicState
                || (a.CompanyId == this.LoginInfo.CompanyId)).
                OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
                return View(pages);
            }
            
        }
        
        public PagedList<Pro2Page> MyWechatSearchPartial(string search = "", int id = 1)
        {
            var result = Searcher.SearchWeChat(search);
            return db.Pro2Page.Where(a => result.Contains(a.PageId) && (a.Page.State == (int)EnumPageState.publicState ||
                    a.CompanyId == FenXiaoUserContext.Current.LoginInfo.CompanyId)).
                OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
        }

        public PagedList<Pro2Page> MyWechatPartial(int wechatid = 0, string luxianname = "", int id = 1)
        {
            
            if (wechatid>0)
            {
                return db.Pro2Page.Where(a => a.Id == wechatid).
                OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            }
            else
            {
                var result = Searcher.Search(luxianname);
                return db.Pro2Page.Where(a => result.Contains(a.ProductId)).
                OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            }
        }







        public ActionResult CreatePage(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult CreatePage(List<string> Images,
            List<string> Subtitles, 
            string Audio,
            string name,
            string state,
            int id)
        {
            
            var page = new FenXiao.Model.Page 
            {
                Audio = Audio,
                CreateTime = DateTime.Now,
                Images = JsonConvert.SerializeObject(Images),
                Subtitle = JsonConvert.SerializeObject(Subtitles),
                Id = Guid.NewGuid(),
                Kind = (int)EnumPageType.one,
                Name = name
            };
            var page2pro = new FenXiao.Model.Pro2Page 
            {
                CompanyId = this.LoginInfo.CompanyId,
                CreateTime = DateTime.Now,
                PageId  = page.Id,
                ProductId = id,
                UserId = this.LoginInfo.UserId
            };
            if (state.Equals("public"))
            {
                page.State = (int)EnumPageState.publicState;
            }
            else
            {
                page.State = (int)EnumPageState.privateState;
            }
            db.Pages.Add(page);
            db.Pro2Page.Add(page2pro);
            db.SaveChanges();
            Searcher.AddWeChat(page);
            return RedirectToAction("DetailList", new { id = page2pro.ProductId });
        }

        public ActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            Page P = db.Pages.Find(id);
            if (P == null)
            {
                return Content("Id无效");
            }
            return View(P);
        }

        public ActionResult Save(Guid id, 
            List<string> Images, 
            List<string> Subtitles,
            string Audio, string name, string state)
        {
            Page P = db.Pages.Find(id);
            if (P == null)
            {
                return Content("Id无效");
            }
            P.Audio = Audio;
            P.Images = JsonConvert.SerializeObject(Images);
            P.Subtitle = JsonConvert.SerializeObject(Subtitles);
            P.Name = name;
            if (state.Equals("public"))
	        {
                P.State=(int)EnumPageState.publicState;
	        }
            else
	        {
                P.State=(int)EnumPageState.privateState;
	        }
            Searcher.DelWeChat(P);
            Searcher.AddWeChat(P);
            db.Entry(P).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DetailList", new { Id = P.Pro2Page.ElementAt(0).ProductId });
        }

        /// <summary>
        /// 删除微信营销页
        /// </summary>
        public ActionResult Del(Guid id)
        {
            Page P = db.Pages.Find(id);
            if (P == null)
            {
                return Content("Id无效");
            }
            Searcher.DelWeChat(P);
            int ProductId = P.Pro2Page.ElementAt(0).ProductId;
            db.Pro2Page.RemoveRange(P.Pro2Page);
            db.Entry<Page>(P).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            
            return RedirectToAction("DetailList", new { Id = ProductId });
        }
    }
}