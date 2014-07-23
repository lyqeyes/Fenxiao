using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class AuthController : AdminControllerBase
    {
        [AuthorizeIgnore]
        public ActionResult Login()
        {
            return View();
        }


        [AuthorizeIgnore]
        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Login(string email, string password)
        {
            var loginInfo = db.Users.FirstOrDefault(a => a.Email == email && a.Password == password);

            if (loginInfo != null)
            {
                this.CookieContext.CompanyId = loginInfo.CompanyId;
                this.CookieContext.UserName = loginInfo.Name;
                this.CookieContext.UserId = loginInfo.Id;
                this.CookieContext.Email = loginInfo.Email;
                this.CookieContext.ImageUrl = loginInfo.ImageUrl;
                this.CookieContext.Role = loginInfo.Role;
                if (loginInfo.Role.Split(',').Contains(((int)EnumRole.guanli).ToString()) ||
                    loginInfo.Role.Split(',').Contains(((int)EnumRole.ziguanli).ToString()))
                {
                    db.LoginInfoes.Add(new LoginInfo
                    {
                        LastActivityTime = DateTime.Now,
                        UserId = loginInfo.Id,
                        CompanyRole = (int)EnumCompany.guanli
                    });
                    db.SaveChanges();
                    return RedirectToAction("Index", "AHome", new { Area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError("error", "没有权限");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("error", "用户名或密码错误");
                return View();
            }
        }

        public ActionResult Logout()
        {
            var user = db.LoginInfoes.FirstOrDefault(a => a.UserId == LoginInfo.UserId);
            db.LoginInfoes.Remove(user);
            db.SaveChanges();
            this.CookieContext.CompanyId = 0;
            this.CookieContext.UserName = null;
            this.CookieContext.UserId = 0;
            this.CookieContext.Email = null;
            this.CookieContext.ImageUrl = null;
            this.CookieContext.Role = null;
            return RedirectToAction("Login");
        }

    }
}