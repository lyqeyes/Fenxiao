using FenXiao.Model;
using FenXiao.Web.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Yeanzhi.System;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class AHelpController : AdminControllerBase
    {
        // GET: Admin/AHelp
        #region HelpTypes
        public ActionResult AllTypes(int? id)
        {
            var helpTypes = db.HelpTypes.OrderBy(a => a.Weight);
            var pages = helpTypes.ToPagedList(id ?? 1, 25);
            return View(pages);
        }

        public string AddType(HelpType newType)
        {
            if (string.IsNullOrEmpty(newType.Name) || newType.Weight == 0)
            {
                return JsonConvert.SerializeObject(new ResultJson()
                {
                    Msg = "属性不能为空",
                    Ok = 0
                });
            }
            if (newType.Id == 0)
            {
                newType.CreateTime = DateTime.Now;
                db.HelpTypes.Add(newType);
                db.SaveChanges();
            }
            else
            {
                newType.CreateTime = DateTime.Now;
                db.Entry(newType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return JsonConvert.SerializeObject(new ResultJson()
            {
                Result = "1",
                Ok = 1
            });
        }
        public string DeleteType(int id)
        {
            var del = db.HelpTypes.FirstOrDefault(a => a.Id == id);
            if (del == null)
            {
                return JsonConvert.SerializeObject(new ResultJson()
                {
                    Msg = "没有该ID对应的类别",
                    Ok = 0
                });
            }
            db.HelpTypes.Remove(del);
            db.SaveChanges();
            return JsonConvert.SerializeObject(new ResultJson()
            {
                Result = "1",
                Ok = 1
            });
        } 
        #endregion

        #region HelpArticel

        /// <summary>
        /// Type下对应的文章
        /// </summary>
        /// <param name="id">Type Id</param>
        /// <returns>article列表</returns>
        public ActionResult Articles2Type(int id)
        {
            var type = db.HelpTypes.FirstOrDefault(a => a.Id == id);
            ViewBag.TypeName = type.Name;
            ViewBag.TypeId = id;
            var articles = db.HelpArticles.Where(a => a.HelpTypeId == id);
            return View(articles);
        }

        public string DeleteArticle(int id)
        {
            var del = db.HelpArticles.FirstOrDefault(a => a.Id == id);
            if (del == null)
            {
                return JsonConvert.SerializeObject(new ResultJson()
                {
                    Msg = "没有该ID对应的文章",
                    Ok = 0
                });
            }
            db.HelpArticles.Remove(del);
            db.SaveChanges();
            return JsonConvert.SerializeObject(new ResultJson()
            {
                Result = "1",
                Ok = 1
            });
        }

        public ActionResult EditArticle(int id, int? typeId) 
        {
            HelpArticle newArticle;
            if (id == 0)
            {
                newArticle = new HelpArticle()
                {
                    Id = 0,
                    HelpTypeId = typeId??1
                };
            }
            else {
                newArticle = db.HelpArticles.FirstOrDefault(a => a.Id == id);
            }
            return View(newArticle);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditArticle(HelpArticle editArticle)
        {
            if(string.IsNullOrEmpty(editArticle.HelpContent))
                editArticle.HelpContent = " "; //文章内容输入为空的情况
            if (editArticle.Id == 0)
            {
                editArticle.CreateTime = DateTime.Now;
                db.HelpArticles.Add(editArticle);
            }
            else {
                editArticle.CreateTime = DateTime.Now;
                db.Entry(editArticle).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Articles2Type", new { id = editArticle.HelpTypeId });
        }

	    #endregion
    }
}