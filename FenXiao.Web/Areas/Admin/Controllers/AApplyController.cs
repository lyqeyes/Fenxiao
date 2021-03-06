﻿using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Admin.Controllers
{
    public class AApplyController : AdminControllerBase
    {
        //
        // GET: /Admin/AApply/
        #region 未处理请求部分
        public ActionResult Index(int? id)
        {
            var applylist = db.Applies.Where(a => a.State == 0).OrderByDescending(a => a.CreateTime);
            return View(applylist.ToPagedList(id ?? 1, 25));
        }
        public ActionResult ApplyDetail(int id)
        {
            var apply = db.Applies.FirstOrDefault(a => a.Id == id);
            return View(apply);
        }
        [HttpPost]
        public ActionResult DealApply(int id, string reason, string Choice)
        {
            lock (LockClass.objDealApply)
            {
                var apply = db.Applies.FirstOrDefault(a => a.Id == id);
                if (apply.State != 0)
                    return RedirectToAction("Index");

                if (Choice == "yes")
                {
                    //添加一条处理信息
                    HandleApply newone = new HandleApply()
                    {
                        UserId = this.CookieContext.UserId,
                        State = (int)EnumApply.Success,
                        ApplyId = id,
                        Reason = reason
                    };
                    db.HandleApplies.Add(newone);
                    db.SaveChanges();

                    //修改当前申请状态
                    apply.State = (int)EnumApply.Success;
                    db.Entry(apply).State = System.Data.Entity.EntityState.Modified;

                    #region 申请部分
                    if (apply.ApplyRole == (int)EnumCompany.pifa || apply.ApplyRole == (int)EnumCompany.lingshou)
                    {
                        //如果请求成为批发商并且被同意 , 则把信息加到该零售商的公司信息中
                        if (apply.ApplyRole == (int)EnumCompany.pifa)
                        {
                            //apply.Company.LvXingSheZeRenXian = apply.LvXingSheZeRenXian;
                            //apply.Company.RenShenYiWaiXian = apply.RenShenYiWaiXian;
                            //apply.Company.RongYuChengHao = apply.RongYuChengHao;
                            //apply.Company.AjiLuXingShe = apply.AjiLuXingShe;
                            //在消息表中添加数据
                            db.Messages.Add(new FenXiao.Model.Message
                            {
                                CreateTime = DateTime.Now,
                                IsRead = 0,
                                MessageContent = "您成为批发商的申请已通过.",
                                State = (int)EnumMessage.chulipifashangshenqing,
                                RelatedId = newone.Id,
                                ToCompanyId = apply.CompanyId
                            });
                        }
                        else if (apply.ApplyRole == (int)EnumCompany.lingshou)
                        {
                            //在消息列表中添加数据
                            db.Messages.Add(new FenXiao.Model.Message
                            {
                                CreateTime = DateTime.Now,
                                IsRead = 0,
                                MessageContent = "您成为零售商的申请已通过.",
                                State = (int)EnumMessage.chulilingshoushangshenqing,
                                RelatedId = newone.Id,
                                ToCompanyId = apply.CompanyId
                            });
                        }

                        db.SaveChanges();

                        //修改公司角色
                        var oldrole = apply.Company.CompanyRole;
                        var newRole = oldrole.Substring(0, oldrole.Length - 1);
                        newRole += apply.ApplyRole.ToString();
                        apply.Company.CompanyRole = newRole;
                        db.Entry(apply.Company).State = System.Data.Entity.EntityState.Modified;
                        //修改公司下所有帐号的角色
                        var acclist = db.Users.Where(a => a.CompanyId == apply.CompanyId).ToList();
                        if (acclist != null)
                        {
                            if (apply.ApplyRole == (int)EnumCompany.pifa)
                            {
                                for (var i = 0; i < acclist.Count; i++ )
                                {
                                    if (acclist[i].Role == ((int)EnumRole.lingshou).ToString())
                                    {
                                        acclist[i].Role = acclist[i].Role + "," + (int)EnumRole.pifa;
                                        }
                                    else
                                    {
                                        acclist[i].Role = acclist[i].Role + "," + (int)EnumRole.zipifa;
                                       }
                                    db.Entry(acclist[i]).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                            else
                            {
                                for(var i = 0; i < acclist.Count; i++)
                                {
                                    if (acclist[i].Role == ((int)EnumRole.pifa).ToString())
                                    {
                                        acclist[i].Role = acclist[i].Role + "," + (int)EnumRole.lingshou;
                                        }
                                    else
                                    {
                                        acclist[i].Role = acclist[i].Role + "," + (int)EnumRole.zilingshou;
                                       }
                                    db.Entry(acclist[i]).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }

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
                    #endregion
                    #region 注册部分
                    else
                    {
                        //修改公司角色
                        if (apply.ApplyRole == (int)EnumCompany.zhucelingshou)
                        {
                            apply.Company.CompanyRole = ((int)EnumCompany.lingshou).ToString();
                            db.Entry(apply.Company).State = System.Data.Entity.EntityState.Modified;
                        }
                        else if (apply.ApplyRole == (int)EnumCompany.zhucepifa)
                        {
                            apply.Company.CompanyRole = ((int)EnumCompany.pifa).ToString();
                            db.Entry(apply.Company).State = System.Data.Entity.EntityState.Modified;
                        }
                        else if (apply.ApplyRole == (int)EnumCompany.zhucelingshoupifa)
                        {
                            apply.Company.CompanyRole = ((int)EnumCompany.lingshou).ToString() + ',' + ((int)EnumCompany.pifa).ToString();
                            db.Entry(apply.Company).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    #endregion
                }
                //申请不通过
                else
                {
                    apply.State = (int)EnumApply.False;
                    db.Entry(apply).State = System.Data.Entity.EntityState.Modified;
                    HandleApply newone = new HandleApply()
                    {
                        UserId = this.CookieContext.UserId,
                        State = (int)EnumApply.False,
                        ApplyId = id,
                        Reason = reason
                    };
                    db.HandleApplies.Add(newone);
                    db.SaveChanges();

                    if (apply.ApplyRole == (int)EnumCompany.pifa || apply.ApplyRole == (int)EnumCompany.lingshou)
                    {
                        //修改公司角色
                        var oldrole = apply.Company.CompanyRole;
                        var newRole = oldrole.Substring(0, oldrole.Length - 2);
                        apply.Company.CompanyRole = newRole;
                        db.Entry(apply.Company).State = System.Data.Entity.EntityState.Modified;

                        //在message表中添加数据
                        string content = "管理员拒绝了您成为" + (apply.ApplyRole == (int)EnumCompany.pifa ? "批发商" : "零售商") + "的申请";
                        int state = apply.ApplyRole == (int)EnumCompany.pifa ? (int)EnumMessage.chulipifashangshenqing : (int)EnumMessage.chulilingshoushangshenqing;
                        db.Messages.Add(new FenXiao.Model.Message
                        {
                            CreateTime = DateTime.Now,
                            IsRead = 0,
                            MessageContent = content,
                            State = state,
                            RelatedId = newone.Id,
                            ToCompanyId = apply.CompanyId
                        });

                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        } 
        #endregion

        #region 已处理请求部分
        public ActionResult DealedApply(int? id)
        {
            var list = db.Applies.Where(a => a.State != 0).OrderByDescending(a => a.CreateTime);
            return View(list.ToPagedList(id??1,25));
        }

        public ActionResult DealedApplyDetail(int id)
        {
            var applyhandle = db.HandleApplies.FirstOrDefault(a => a.ApplyId == id);
            return View(applyhandle);
        }

        #endregion
    }
}