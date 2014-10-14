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
    public class ASiteManagerController : AdminControllerBase
    {
        public override int PageSize
        {
            get
            {
                return 15;
            }
        }
        // GET: Admin/ASiteManager
        public ActionResult Index(int id = 1)
        {
            var list = db.Users.Where(a => a.Role.Contains("0") || a.Role.Contains("1")).OrderBy(a => a.Role);
            var page = list.ToPagedList(id, PageSize);
            if (UserContext.LoginInfo.Role.Contains(0))
            {
                //返回总管理员视图
                return View(page);
            }
            else
            {
                //返回子管理员人员视图
                return View("index2",page);
            }
        }
        //冻结或者解冻帐号
        public string ChangeAccountState(int id)
        {
            var changeone = db.Users.Find(id);
            if (!LoginInfo.Role.Contains(0))
            {
                return JsonConvert.SerializeObject(new ResultJson()
                {
                    Msg = "非总管理员, 无法执行操作",
                    Ok = 0
                });
            }
            else if (changeone.Role.Contains("0"))
            {
                return JsonConvert.SerializeObject(new ResultJson()
                {
                    Msg = "无法冻结总管理员",
                    Ok = 0
                });
            }
            else {
                changeone.State = changeone.State == (int)EnumUser.zhengchang ? (int)EnumUser.dongjie : (int)EnumUser.zhengchang;
                db.Entry(changeone).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return JsonConvert.SerializeObject(new ResultJson()
                {
                    Msg = "操作成功",
                    Ok = 1
                });
            }
        }
    }
}