using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Yeanzhi.System;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class ASiteManagerController : AdminControllerBase
    {
        public int SiteManagerCompanyId {
            get {
                return 10;
            }
        }
        public string ZiguanliMorenImage
        {
            get {
                return "http://210.30.100.181:8888/Upload/default/day140612/201406121103027652.jpg";
            }
        }
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
                //返回子管理员视图
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

        //创建帐号
        public ActionResult EditAccount(int? id)
        {
            var theid = id ?? 0;
            if (theid == 0)
            {
                return View(new User { });
            }
            else
            {
                var editone = db.Users.FirstOrDefault(a => a.Id == id);
                if (editone != null)
                {
                    return View(editone);
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult EditAccount(User newone)
        {
            //如果是新建
            if (newone.Id == 0)
            {
                newone.CreateTime = DateTime.Now;
                newone.CompanyId = SiteManagerCompanyId;
                newone.State = (int)EnumUser.zhengchang;
                newone.Role = ((int)EnumRole.ziguanli).ToString();
                newone.ImageUrl = ZiguanliMorenImage;
                db.Users.Add(newone);
                db.SaveChanges();
            }
            else  //如果是编辑
            {
                var admin = db.Users.Find(newone.Id);
                admin.Phone = newone.Phone;
                admin.Email = newone.Email;
                admin.Password = newone.Password;
                admin.Name = newone.Name;
                db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                //db.Entry(newone).State = System.Data.Entity.EntityState.Unchanged;
                //db.Entry(newone).Property(model => model.Phone).IsModified = true;
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    StringBuilder errors = new StringBuilder();
                    IEnumerable<DbEntityValidationResult> validationResult = ex.EntityValidationErrors;
                    foreach (DbEntityValidationResult result in validationResult)
                    {
                        ICollection<DbValidationError> validationError = result.ValidationErrors;
                        foreach (DbValidationError err in validationError)
                        {
                            errors.Append(err.PropertyName + ":" + err.ErrorMessage + "\r\n");
                        }
                    }
                    Debug.WriteLine(errors.ToString());
                }

                
            }
            return RedirectToAction("Index");
        }
    }
}