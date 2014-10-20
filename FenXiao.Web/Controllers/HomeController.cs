using FenXiao.Web.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yeanzhi.System;
//using Yeanzhi.System.Upload;
using Qiniu.Conf;
using Qiniu.RS;
using Qiniu.IO;
using FenXiao.Model;
using System.Threading;
using FenXiao.Web.Models;
using FenXiao.Web.Common;

namespace FenXiao.Web.Controllers
{
    public class HomeController : Controller
    {
        public FenXiaoCookieContext CookieContext
        {
            get
            {
                return FenXiaoCookieContext.Current;
            }
        }

        public ActionResult Index()
        {
            return RedirectToAction("Login", "MAuth", new { Area = "Marketer" });
            //if (this.CookieContext.CompanyRole==-1)
            //{
            //    return RedirectToAction("Login", "MAuth", new { Area = "Marketer" });
            //}
            //else if (this.CookieContext.CompanyRole==(int)EnumCompany.lingshou)
            //{
            //    return RedirectToAction("LineSearch", "MSearch", new { Area = "Marketer" });
            //}
            //else
            //{
            //    return RedirectToAction("MyLuXian", "WHome", new { Area = "Wholesaler" });
            //}
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region 跑马灯数据源
        [HttpPost]
        public ActionResult RunLight()
        {
            using (FenXiaoDBEntities db = new FenXiaoDBEntities())
            {
                var NowTime = DateTime.Now.Date;
                var list = db.Ads.Where(a => a.StartTime <= NowTime && a.EndTime >= NowTime).OrderByDescending(a=>a.CreateTime).Select(a =>a.AdContent).ToList();
                return Json(list);
            }
        }
        #endregion
        public ActionResult DownLuXian(int id)
        {
            using (FenXiaoDBEntities db = new FenXiaoDBEntities())
            {
                var Product = db.Products.Find(id);
                if (Product == null)
                {
                    return HttpNotFound();
                }
                try
                {
                    var fujian = JsonConvert.DeserializeObject<List<fujianDto>>(Product.FuJian);
                    return View(fujian);
                }
                catch (Exception)
                {
                    return View(new List<fujianDto>());
                }
                
            }
        }
        public ActionResult YuLan(Guid id)
        {
            using (FenXiaoDBEntities db = new FenXiaoDBEntities())
            {
                var page = db.Pages.Find(id);
                if (page == null)
                {
                    return HttpNotFound();
                }
                return View(page);
            }
        }
        public string Ajax()
        {
            int count = 0;
            for (int i = 0; i < 10000000; i++)
            {
                for (int j = 0; j < 100; j++)
                    count += j;
                count += i * 2;
            }
            return count.ToString();
        }
        public string Upload()
        {
            var v = new UpImageHelper();
            return v.Upload(System.Web.HttpContext.Current);
        }
        //图片上传
        [HttpPost]
        public JsonResult ImageUp()
        {
            var filecollection = Request.Files;
            var fileData = filecollection[0];
            Qiniu.Conf.Config.ACCESS_KEY = "o6njB-MWVByGvLlO5QfAO5r1yxQ2YToBaL99uBlj";
            Qiniu.Conf.Config.SECRET_KEY = "1YFm-nOr3MQkgkiTRg3bzwHZwI4zd9OZo8xfRQMr";
            string bucketName = "ouredaimage";
            PutPolicy put = new PutPolicy(bucketName);
            string uptoken = put.Token();
            PutExtra extra = new PutExtra();
            IOClient client = new IOClient();
            var ext = fileData.FileName.Substring(fileData.FileName.LastIndexOf('.') + 1).ToLower();
            var result = client.Put(uptoken, Guid.NewGuid().ToString() + "." + ext, fileData.InputStream, extra);
            var url =  "http://ouredaimage.qiniudn.com/" + result.key;
            //上传配置
            return Json(new { url = url, title = fileData.FileName, fileData.FileName, state = "SUCCESS" });
        }
        //public string Upload1()
        //{
        //    var filecollection = Request.Files;
        //    HttpPostedFileBase fileData = filecollection[0];
        //     Yeanzhi.System.Config.ACCESS_KEY = "1d98b725-0633-4928-8a14-182b21586daa";
        //     Yeanzhi.System.Config.SECRET_KEY = "d8ed80a8-ca5b-40ad-9899-209d52bca6da";
        //    IOClient ioClient = new IOClient();
        //    PutExtra put = new PutExtra(fileData.ContentType.ToString());
        //    var v = ioClient.Put(Guid.NewGuid().ToString(), fileData, put);
        //    var res = JsonConvert.DeserializeObject<result>(v.Response);
        //    return res.url;
        //}
        public class result
        {
            public string localname { get; set; }
            public string url { get; set; }
            public string err { get; set; }
        }

        public ActionResult AnLi()
        {
            return View();
        }

        public ActionResult NoPermission()
        {
            return View();
        }

        public ActionResult LoginOutTime()
        {
            return View();
        }
    }
}