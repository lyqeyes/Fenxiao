using FenXiao.Model;
using FenXiao.Web.Areas.Wholesaler.Models;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MAuthController : MarketerControllerBase
    {
        [AuthorizeIgnore]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Login(string email, string password, string type)
        {
            //TODO 需要判断及所属公司的状态，是冻结还是正常。未对登录异常做页面展示
            if (type == "pfs")
            {
                var loginInfo = db.Users.FirstOrDefault(a => a.Email == email && a.Password == password);
                if (loginInfo != null)
                {
                    if (loginInfo.State == (int)EnumUser.dongjie)
                    {
                        ModelState.AddModelError("error", "该账户已被冻结");
                        return View();
                    }
                    this.CookieContext.CompanyId = loginInfo.CompanyId;
                    this.CookieContext.UserName = loginInfo.Name;
                    this.CookieContext.UserId = loginInfo.Id;
                    this.CookieContext.Email = loginInfo.Email;
                    this.CookieContext.ImageUrl = loginInfo.ImageUrl;
                    this.CookieContext.Role = loginInfo.Role;
                    this.CookieContext.CompanyRole = (int)EnumCompany.pifa;
                    if (loginInfo.Role.Split(',').Contains(((int)EnumRole.pifa).ToString()) ||
                        loginInfo.Role.Split(',').Contains(((int)EnumRole.zipifa).ToString()))
                    {
                        var lgs = db.LoginInfoes.Where(a => a.UserId == loginInfo.Id);
                        db.LoginInfoes.RemoveRange(lgs);
                        db.SaveChanges();
                        db.LoginInfoes.Add(new LoginInfo
                        {
                            LastActivityTime = DateTime.Now,
                            UserId = loginInfo.Id,
                            CompanyRole = (int)EnumCompany.pifa
                        });
                        db.SaveChanges();
                        return RedirectToAction("MyLuXian", "WHome", new { Area = "Wholesaler" });
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
            else
            {
                var loginInfo = db.Users.FirstOrDefault(a => a.Email == email && a.Password == password);
                if (loginInfo != null)
                {
                    if (loginInfo.State == (int)EnumUser.dongjie)
                    {
                        ModelState.AddModelError("error", "该账户已被冻结");
                        return View();
                    }
                    this.CookieContext.CompanyId = loginInfo.CompanyId;
                    this.CookieContext.UserName = loginInfo.Name;
                    this.CookieContext.UserId = loginInfo.Id;
                    this.CookieContext.Email = loginInfo.Email;
                    this.CookieContext.ImageUrl = loginInfo.ImageUrl;
                    this.CookieContext.Role = loginInfo.Role;
                    this.CookieContext.CompanyRole = (int)EnumCompany.lingshou;
                    if (loginInfo.Role.Split(',').Contains(((int)EnumRole.lingshou).ToString()) ||
                        loginInfo.Role.Split(',').Contains(((int)EnumRole.zilingshou).ToString()))
                    {
                        var loglist = db.LoginInfoes.Where(a => a.UserId == loginInfo.Id);
                        db.LoginInfoes.RemoveRange(loglist);
                        db.SaveChanges();
                        db.LoginInfoes.Add(new LoginInfo
                        {
                            LastActivityTime = DateTime.Now,
                            UserId = loginInfo.Id,
                            CompanyRole = (int)EnumCompany.lingshou
                        });
                        db.SaveChanges();
                        return RedirectToAction("LineSearch", "MSearch", new { Area = "Marketer" });
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
        }

        public ActionResult ConvertRole()
        {
            if (!(LoginInfo.Role.Contains((int)EnumRole.pifa) || LoginInfo.Role.Contains((int)EnumRole.zipifa)))
            {
                return RedirectToAction("NoRole");
            }
            else
            {
                var login = db.LoginInfoes.FirstOrDefault(a => a.UserId == this.LoginInfo.UserId);
                login.CompanyRole = (int)EnumCompany.pifa;
                db.Entry(login).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                this.CookieContext.CompanyRole = (int)EnumCompany.pifa;
                return RedirectToAction("MyLuXian", "WHome", new { Area = "Wholesaler" });
            }
        }

        public ActionResult Logout()
        {
            var users = db.LoginInfoes.Where(a => a.UserId == LoginInfo.UserId);
            db.LoginInfoes.RemoveRange(users);
            db.SaveChanges();
            this.CookieContext.CompanyId = 0;
            this.CookieContext.UserName = String.Empty;
            this.CookieContext.UserId = 0;
            this.CookieContext.Email = String.Empty;
            this.CookieContext.ImageUrl = String.Empty;
            this.CookieContext.Role = String.Empty;
            return RedirectToAction("Login");
        }

        public ActionResult EditInfo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditInfo(RestPass rp)
        {
            if (rp.newpass != rp.newpasstwo)
            {
                ViewBag.error = "两次输入的密码不一致";
                return View(rp);
            }
            else
            {
                var user = db.Users.Find(LoginInfo.UserId);
                if (user == null)
                {
                    return HttpNotFound();
                }
                else if (user.Password != rp.oldpass)
                {
                    ViewBag.error = "旧密码错误";
                    return View(rp);
                }
                else
                {
                    user.Password = rp.newpasstwo;
                    db.Users.Attach(user);

                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Member", "MHome", new { Area = "Marketer" });
                }
            }
        }

        public ActionResult EditEmail()
        {
            var user = db.Users.Find(LoginInfo.UserId);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult EditEmail(User user)
        {

            var dbuser = db.Users.Find(LoginInfo.UserId);
            if (dbuser == null)
            {
                return HttpNotFound();
            }
            else
            {
                dbuser.Phone = user.Phone;
                dbuser.Name = user.Name;
                dbuser.Email = user.Email;
                db.Users.Attach(dbuser);
                db.Entry(dbuser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Member", "MHome", new { Area = "Marketer" });
            }
        }

        #region 修改资料后等待管理员审核
        [HttpGet]
        public ActionResult Wait()
        {
            return View();
        }
        #endregion
    }
}