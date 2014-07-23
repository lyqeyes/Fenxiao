using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
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

                    //如果请求成为批发商并且被同意 , 则把信息加到该零售商的公司信息中
                    if (apply.ApplyRole == (int)EnumCompany.pifa)
                    {
                        apply.Company.LvXingSheZeRenXian = apply.LvXingSheZeRenXian;
                        apply.Company.RenShenYiWaiXian = apply.RenShenYiWaiXian;
                        apply.Company.RongYuChengHao = apply.RongYuChengHao;
                        apply.Company.AjiLuXingShe = apply.AjiLuXingShe;
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
                    else { 
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
                    //修改当前申请状态
                    apply.State = (int)EnumApply.Success;
                    db.Entry(apply).State = System.Data.Entity.EntityState.Modified;

                    //修改公司角色
                    var oldrole = apply.Company.CompanyRole;
                    var newRole = oldrole.Substring(0, oldrole.Length - 1);
                    newRole += apply.ApplyRole.ToString();
                    apply.Company.CompanyRole = newRole;
                    db.Entry(apply.Company).State = System.Data.Entity.EntityState.Modified;
                    //修改公司下所有帐号的角色
                    var acclist = db.Users.Where(a => a.CompanyId == apply.CompanyId);
                    if (acclist != null)
                    {
                        if (apply.ApplyRole == (int)EnumCompany.pifa)
                        {
                            foreach (var item in acclist)
                            {
                                if (item.Role == ((int)EnumRole.lingshou).ToString())
                                {
                                    item.Role = item.Role + "," + (int)EnumRole.pifa;
                                }
                                else
                                {
                                    item.Role = item.Role + "," + (int)EnumRole.zipifa;
                                }
                                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        else {
                            foreach (var item in acclist)
                            {
                                if (item.Role == ((int)EnumRole.pifa).ToString())
                                {
                                    item.Role = item.Role + "," + (int)EnumRole.lingshou;
                                }
                                else
                                {
                                    item.Role = item.Role + "," + (int)EnumRole.zilingshou;
                                }
                                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }

                    db.SaveChanges();
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