using FenXiao.Model;
using FenXiao.Web.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MWeChatController : MarketerControllerBase
    {
        public ActionResult Management(int id = 0)
        {
            var Products = db.Products.Where(a => a.User.CompanyId ==
                this.LoginInfo.CompanyId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(Products);
        }

        public ActionResult DetailList(int id)
        {
            var pro = db.Pro2Page.Where(
                a => a.CompanyId == this.LoginInfo.CompanyId &&
                    a.ProductId == id).ToList();
            return View(pro);
        }

        public ActionResult Index(int id = 0)
        {
            var pages = db.Pro2Page.Where(a =>
                a.Page.State == (int)EnumPageState.publicState
                || (a.CompanyId == this.LoginInfo.CompanyId)).
                OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(pages);
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
                PageId = page.Id,
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
                P.State = (int)EnumPageState.publicState;
            }
            else
            {
                P.State = (int)EnumPageState.privateState;
            }
            db.Entry(P).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DetailList", new { Id = P.Pro2Page.ElementAt(0).ProductId });
        }

    }
}