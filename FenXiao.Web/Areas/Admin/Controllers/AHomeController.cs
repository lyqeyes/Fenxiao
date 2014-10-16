using FenXiao.Model;
using FenXiao.Web.Areas.Admin.Models;
using FenXiao.Web.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class AHomeController : AdminControllerBase
    {
        // GET: Admin/AHome
        public ActionResult Index()
        {
            //1.未处理的申请
            var todeallist = db.Applies.Where(a => a.State == (int)EnumApply.Applying).ToList();
            var todealnum = todeallist.Count;
            //2. 新的线路
            var today = DateTime.Now;
            //var newLinelist = db.Products.Where(a => (today - a.CreateTime).Days <= 3).ToList();
            var newLinelist = db.Products.Where(a => (DbFunctions.DiffDays(a.CreateTime, today) <= 3)).ToList();
            var newlinenum = newLinelist.Count;

            //赋值
            IndexModel result = new IndexModel();
            result.NewApplyNum = todealnum;
            result.NewLineNum = newlinenum;
            if (todealnum > 8)
            {
                result.NewApplyList = todeallist.Take(8).ToList();
            }
            else {
                result.NewApplyList = todeallist.ToList();
            }
            if (newlinenum > 8)
            {
                result.NewLineList = newLinelist.Take(8).ToList();
            }
            else
            {
                result.NewLineList = newLinelist.ToList();
            }

            return View(result);
        }
    }
}