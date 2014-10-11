using FenXiao.Model;
using FenXiao.Web.Areas.Admin.Models;
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
    public class AAdvertiseController : AdminControllerBase
    {
        // GET: Admin/AAdvertise
        
        //所有广告
        public ActionResult AllAdvertise(int? id)
        {
            var ads = (from ad in db.Ads
                       join u in db.Users on ad.CreateUserId equals u.Id
                       select new AdItem
                       {
                           Id = ad.Id,
                           StartTime = ad.StartTime,
                           EndTime = ad.EndTime,
                           CreateTime = ad.CreateTime,
                           AdContent = ad.AdContent,
                           CreateUserName = u.Name
                       }
                      );
            var list = ads.OrderByDescending(a => a.CreateTime).ToPagedList(id ?? 1, 25);
            return View(list);
        }

        //创建与编辑广告
        public ActionResult EditAd(long? id)
        {
            var theid = id ?? 0;
            if (theid == 0)
            {
                return View(new Ad { });
            }
            else
            {
                var editone = db.Ads.FirstOrDefault(a => a.Id == id);
                if (editone != null)
                {
                    return View(editone);
                }
                return RedirectToAction("AllAdvertise");
            }
        }
        [HttpPost]
        public ActionResult EditAd(Ad newone)
        {
            //如果是新建
            if (newone.Id == 0)
            {
                newone.CreateTime = DateTime.Now;
                newone.CreateUserId = UserContext.LoginInfo.UserId;
                db.Ads.Add(newone);
                db.SaveChanges();
            }
            else  //如果是编辑
            {
                newone.CreateTime = DateTime.Now;
                newone.CreateUserId = UserContext.LoginInfo.UserId;
                db.Entry(newone).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("AllAdvertise");
        }

        //删除广告
        public string DeleteAd(long id)
        {
            var delone = db.Ads.FirstOrDefault(a=>a.Id == id);
            if (delone != null)
            {
                db.Ads.Remove(delone);
                db.SaveChanges();
                return JsonConvert.SerializeObject(new ResultJson()
                {
                    Msg = "删除成功",
                    Ok = 1
                });
            }
            else {
                return JsonConvert.SerializeObject(new ResultJson()
                {
                    Msg = "不存在该条广告",
                    Ok = 0
                });
            }
        }
    }
}