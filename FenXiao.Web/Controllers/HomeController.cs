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

namespace FenXiao.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return RedirectToAction("Login", "MAuth", new { Area = "Marketer" });
            //return View();
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
            var filecollection = Request.Files;
            HttpPostedFileBase fileData = filecollection[0];
            Qiniu.Conf.Config.ACCESS_KEY = "o6njB-MWVByGvLlO5QfAO5r1yxQ2YToBaL99uBlj";
            Qiniu.Conf.Config.SECRET_KEY = "1YFm-nOr3MQkgkiTRg3bzwHZwI4zd9OZo8xfRQMr";
            string bucketName = "ouredaimage";
            PutPolicy put = new PutPolicy(bucketName);
            string uptoken = put.Token();
            PutExtra extra = new PutExtra();
            IOClient client = new IOClient();
            var ext = fileData.FileName.Substring(fileData.FileName.LastIndexOf('.') + 1).ToLower();
            var result = client.Put(uptoken, Guid.NewGuid().ToString() + "."+ext, fileData.InputStream, extra);
            return "http://ouredaimage.qiniudn.com/" + result.key;
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
    }
}