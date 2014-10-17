﻿using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Marketer.Controllers
{
    /// <summary>
    /// 公司管理
    /// </summary>
    public class MCompanyController : MarketerControllerBase
    {
        //内容头部的公司信息
        [HttpGet]
        public ActionResult CompanyInfo()
        {
            ViewBag.Count = db.Users.Count(e => e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId) - 1;
            ViewBag.IsAll = FenXiaoUserContext.Current.UserInfo.Role.Split(',').Contains(((int)EnumRole.lingshou).ToString());
            var userList = db.Users.Where(e => e.CompanyId == FenXiaoUserContext.Current.UserInfo.CompanyId);
            foreach (var user in userList)
            {
                if (user.Role.Split(',').Contains(((int)EnumRole.lingshou).ToString()))
                {
                    ViewBag.Name = user.Name;
                    break;
                }
            }
            return PartialView();
        }

        //公司管理首页
        [HttpGet]
        public ActionResult Index(int id = 0)
        {
            var userlist = db.Users.Where(a => a.CompanyId == LoginInfo.CompanyId).OrderBy(a => a.Id).ToPagedList(id, PageSize);
            return View(userlist);
        }

        //创建子账户
        [HttpGet]
        public ActionResult CreateMember()
        {
            return View();
        }
        [HttpPost]

        public ActionResult CreateMember([Bind(Include = "Password,Name,Phone,Email")]User newUser)
        {
            var user = db.Users.FirstOrDefault(a => a.Email == newUser.Email);
            if (user != null)
            {
                //TODO 没有处理异常
                ViewBag.error = "此邮箱已存在";
                return View(newUser);
            }
            db.Users.Add(new User
            {
                CompanyId = LoginInfo.CompanyId,
                CreateTime = DateTime.Now,
                Role = ((int)EnumRole.zilingshou).ToString(),
                Email = newUser.Email,
                ImageUrl = "",
                Name = newUser.Name,
                Password = newUser.Password,
                State = (int)EnumUser.zhengchang,
                Phone = newUser.Phone,
            });
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        //编辑子账户
        [HttpGet]
        public ActionResult EditMember(int id, int ReferIndex=0)
        {
            var user = db.Users.Find(id);
            ViewBag.ReferIndex = ReferIndex;
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult EditMember([Bind(Include = "Password,Name,Phone,Email,Id")]User newUser, int ReferIndex=0)
        {
            var user = db.Users.Find(newUser.Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Name = newUser.Name;
            user.Phone = newUser.Phone;
            user.Email = newUser.Email;
            user.Password = newUser.Password;
            db.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = ReferIndex });
        }

        //修改子账户状态
        [HttpGet]
        public ActionResult EditMemberState(int id, string Type, int ReferIndex=0)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            switch (Type)
            {
                case "恢复":
                    {
                        user.State = (int)EnumUser.zhengchang;
                        break;
                    }
                case "冻结":
                    {
                        user.State = (int)EnumUser.dongjie;
                        break;
                    }
                default:
                    {
                        return HttpNotFound();
                    }
            }
            db.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = ReferIndex});
        }

        //修改公司信息
        public ActionResult EditCompany()
        {
            var com = db.Companies.FirstOrDefault(a => a.Id == this.LoginInfo.CompanyId);
            if (com == null)
            {
                return HttpNotFound();
            }
            return View(com);
        }

        [HttpPost]
        public ActionResult EditCompany(TempCompany tempCompany)
        {
            if (!this.LoginInfo.Role.Contains((int)EnumRole.lingshou))
            {
                return HttpNotPermission();
            }
            var com = db.Companies.FirstOrDefault(a => a.Id == this.LoginInfo.CompanyId);
            if (com == null)
            {
                return HttpNotFound();
            }
            tempCompany.State = 1;
            tempCompany.CompanyId = this.LoginInfo.CompanyId;
            db.TempCompanies.Add(tempCompany);
            db.SaveChanges();
            Response.AppendHeader("Cache-Control", "no-cache");
            return RedirectPermanent("~/Marketer/MAuth/Login");
        }
        //申请成为批发商
        //[HttpGet]
        //public ActionResult ApplyPiFa()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult ApplyPiFa(string LvXingSheZeRenXian, string RenShenYiWaiXian, string AjiLuXingShe, string RongYuChengHao)
        //{
        //    lock (LockClass.objApplyToPiFa)
        //    {
        //        if (FenXiaoUserContext.Current.UserInfo.Company.CompanyRole.Contains(((int)EnumCompany.zanshipifa).ToString()))
        //        {
        //            return View();
        //        }
        //        var apply = new Apply();
        //        apply.LvXingSheZeRenXian = LvXingSheZeRenXian;
        //        apply.RenShenYiWaiXian = RenShenYiWaiXian;
        //        apply.AjiLuXingShe = AjiLuXingShe;
        //        apply.RongYuChengHao = RongYuChengHao;
        //        apply.CreateTime = DateTime.Now;
        //        apply.CompanyId = FenXiaoUserContext.Current.UserInfo.CompanyId;
        //        apply.ApplyRole = (int)EnumCompany.pifa;
        //        apply.State = (int)EnumApply.Applying;
        //        db.Entry<Apply>(apply).State = System.Data.Entity.EntityState.Added;
        //        FenXiaoUserContext.Current.UserInfo.Company.CompanyRole += ("," + ((int)EnumCompany.zanshipifa).ToString());
        //        db.Entry<Company>(FenXiaoUserContext.Current.UserInfo.Company).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        #region 申请成为批发商
        public ActionResult ReplyToPiFa()
        {
            return View();
        }

        public ActionResult SubmitReply()
        {
            if (!UserContext.UserInfo.Company.
                 CompanyRole.Split(',').
                 Contains(((int)EnumCompany.zanshipifa).ToString()))
            {
                lock (LockClass.objApplyToLingShou)
                {
                    if (!UserContext.UserInfo.Company.
                     CompanyRole.Split(',').
                     Contains(((int)EnumCompany.zanshipifa).ToString()))
                    {
                        db.Applies.Add(new Apply
                        {
                            ApplyRole = (int)EnumCompany.pifa,
                            CompanyId = UserContext.UserInfo.CompanyId,
                            CreateTime = DateTime.Now,
                            State = (int)EnumApply.Applying
                        });
                        var com = db.Companies.Find(UserContext.UserInfo.CompanyId);
                        com.CompanyRole = ((int)EnumCompany.lingshou) + "," + ((int)EnumCompany.zanshipifa);
                        db.Companies.Attach(com);
                        db.Entry(com).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region 消息管理
        //消息列表
        public ActionResult Message(int id = 0)
        {
            var msgs = db.Messages.Where(a => a.ToCompanyId == FenXiaoUserContext.Current.LoginInfo.CompanyId
                 &&
                (a.State == (int)EnumMessage.chulituidan ||
                a.State == (int)EnumMessage.chulipifashangshenqing)).OrderByDescending(a => a.CreateTime).ToPagedList(id, PageSize);
            return View(msgs);
        }

        public ActionResult Read(int id)
        {
            var message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            else
            {
                MessageContext.IsRead(message.Id);
                if (message.State == (int)EnumMessage.chulituidan)
                {
                    return RedirectToAction("CancelOrderDetail", "MLine", new { Area = "Marketer", id = message.RelatedId });

                }
                else
                {
                    return RedirectToAction("HandleComInfoResult", "MCompany", new { Area = "Marketer", id = message.RelatedId });
                }
            }
        }

        //公司申请信息结果
        public ActionResult HandleComInfoResult(int id)
        {
            var v = db.HandleApplies.Find(id);
            if (v == null)
            {
                return HttpNotFound();
            }
            return View(v);
        }
        #endregion
    }
}