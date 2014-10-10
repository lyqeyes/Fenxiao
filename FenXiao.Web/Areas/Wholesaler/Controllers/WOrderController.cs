using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;


namespace FenXiao.Web.Areas.Wholesaler.Controllers
{
    /// <summary>
    /// 订单处理记录，未处理订单，已处理订单（各类搜索）
    /// </summary>
    public class WOrderController : WholesalerControllerBase
    {
        /// <summary>
        /// 占位订单
        /// </summary>
        /// <returns></returns>
        public ActionResult HandlingOrderView(int id=0)
        {
            var order = db.OrderForms.Where(a =>
                a.ToCompanyId == LoginInfo.CompanyId &&
                a.State == (int)EnumOrderForm.ReserveNowOrder ).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            
            return View(order);
        }

        /// <summary>
        /// 占位订单搜索
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult HandlingOrderParialView(int orderid = 0)
        {
            var v = db.OrderForms.FirstOrDefault(a => a.Id == orderid &&
                a.ToCompanyId == LoginInfo.CompanyId&&a.State==(int)EnumOrderForm.ReserveNowOrder);
            return View(v);
        }
        /// <summary>
        /// 直接订单
        /// </summary>
        /// <returns></returns>
        public ActionResult HandledOrderView(int id = 0)
        {
            var order = db.OrderForms.Where(a =>
                a.ToCompanyId == LoginInfo.CompanyId &&
                a.State != (int)EnumOrderForm.ReserveNowOrder).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(order);
        }

        public ActionResult HandledOrderParialView(int orderid = 0)
        {
            var v = db.OrderForms.FirstOrDefault(a=>a.Id==orderid&&
                a.ToCompanyId == LoginInfo.CompanyId && a.State != (int)EnumOrderForm.ReserveNowOrder);
            return View(v);
        }

        public ActionResult HandleOrder(int id)
        {
            var order = db.OrderForms.FirstOrDefault(a => a.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            if (order.State == (int)EnumOrderForm.xiadingdan)
            {
                lock (LockClass.objOrder)
                {
                    if (order.State == (int)EnumOrderForm.xiadingdan)
                    {
                        order.State = (int)EnumOrderForm.chulidingdan;
                        db.OrderForms.Attach(order);
                        db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        db.Messages.Add(new FenXiao.Model.Message
                        {
                            CreateTime = DateTime.Now,
                            IsRead = 0,
                            RelatedId = order.Id,
                            State = (int)EnumMessage.chulidingdan,
                            ToCompanyId = order.User.CompanyId
                        });
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("HandlingOrderView");
        }

        public ActionResult OrderDetial(int id)
        {
            var order = db.OrderForms.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            PermissionCompanyId = order.ToCompanyId;
            return View(order);
        }





        /// <summary>
        /// 待处理退订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult HandlingReturnOrderView(int id = 0)
        {
            var order = db.ReturnForms.Where(a =>
                a.ToCompanyId == LoginInfo.CompanyId &&
                a.State == (int)EnumReturnForm.xiatuidan).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(order);
        }

        public ActionResult HandlingReturnOrderParialView(int orderid = 0)
        {
            var v = db.ReturnForms.FirstOrDefault(a => a.Id == orderid &&
                a.ToCompanyId == LoginInfo.CompanyId && a.State == (int)EnumReturnForm.xiatuidan);
            return View(v);
        }

        /// <summary>
        /// 已处理退订单
        /// </summary>
        /// <returns></returns>
        public ActionResult HandledReturnOrderView(int id = 0)
        {
            var order = db.ReturnForms.Where(a =>
               a.ToCompanyId == LoginInfo.CompanyId &&
               a.State != (int)EnumReturnForm.xiatuidan).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(order);
        }

        public ActionResult HandledReturnOrderParialView(int orderid = 0)
        {
            var v = db.ReturnForms.FirstOrDefault(a => a.Id == orderid &&
                a.ToCompanyId == LoginInfo.CompanyId && a.State != (int)EnumReturnForm.xiatuidan);
            return View(v);
        }

        public ActionResult ReturnOrderDetial(int id)
        {
            var reor = db.ReturnForms.Find(id);
            if (reor == null)
            {
                return HttpNotFound();
            }
            PermissionCompanyId = reor.ToCompanyId;
            if (!string.IsNullOrEmpty(reor.CustomerList))
            {
                var cl = reor.CustomerList.Split('+').Select(a=>Convert.ToInt64(a)).ToList();
                var cus = db.CustomerInfoes.Where(a => cl.Contains(a.Id));
                ViewBag.Count = cus.Count();
                ViewBag.CustomerInfoes = cus;
            }
            return View(reor);
        }

        public ActionResult HandleReturnPage(int id, string state)
        {
            var rm = db.ReturnForms.Find(id);
            if (rm == null)
            {
                return HttpNotFound();
            }
            if (state == "ok")
            {
                ViewBag.State = "通过";
            }
            else
            {
                ViewBag.State = "拒绝";
            }
            ViewBag.inputstr = state;
            PermissionCompanyId = rm.ToCompanyId;
            return View(rm);
        }

        public ActionResult HandleReturnOrder(int id, string state, string note)
        {
            var retf = db.ReturnForms.Find(id);
            if (retf == null)
            {
                return HttpNotFound();
            }
            lock (LockClass.GetReturnFormLock(retf.Id))
            {
                var ReturnForm = db.ReturnForms.Find(id);
                var cp = db.ChildProducts.FirstOrDefault(a => a.ProductId == ReturnForm.ProductId);
                if (cp == null)
                {
                    return HttpNotFound();
                }
                if (ReturnForm.State == (int)EnumReturnForm.xiatuidan)
                {
                    if (state == "ok")
                    {
                        ReturnForm.State = (int)EnumReturnForm.chulidingdan;
                        db.ReturnForms.Attach(ReturnForm);
                        db.Entry(ReturnForm).State = System.Data.Entity.EntityState.Modified;
                        cp.AllCount -= ReturnForm.AllCount;
                        if (!string.IsNullOrEmpty(ReturnForm.CustomerList))
                        {
                            cp.ZhanWeiLockCount -= (ReturnForm.AllCount - ReturnForm.CustomerList.Split('+').Count());
                            foreach (var Id in ReturnForm.CustomerList.Split(','))
                            {
                                var customerInfo = db.CustomerInfoes.Find(int.Parse(Id));
                                customerInfo.State = (int)EnumCustomer.YiShanChu;
                                db.Entry<CustomerInfo>(customerInfo).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        else
                        {
                            cp.ZhanWeiLockCount -= ReturnForm.AllCount;
                        }

                        db.ChildProducts.Attach(cp);
                        db.Entry(cp).State = System.Data.Entity.EntityState.Modified;
                        db.HandleReturnForms.Add(
                            new HandleReturnForm
                            {
                                CreateTime = DateTime.Now,
                                Reason = note,
                                ReturnFormId = ReturnForm.Id,
                                UserId = LoginInfo.UserId
                            });
                        db.Messages.Add(new FenXiao.Model.Message
                        {
                            CreateTime = DateTime.Now,
                            IsRead = 0,
                            RelatedId = ReturnForm.Id,
                            State = (int)EnumMessage.chulituidan,
                            MessageContent = string.Format("退订单{0}，名称{1},数量{2}已被通过",
                            ReturnForm.Id,ReturnForm.Name,ReturnForm.AllCount),
                            ToCompanyId = ReturnForm.User.CompanyId
                        });
                        db.SaveChanges();
                    }
                    else
                    {
                        ReturnForm.State = (int)EnumReturnForm.quxiaodingdan;
                        db.ReturnForms.Attach(ReturnForm);
                        db.Entry(ReturnForm).State = System.Data.Entity.EntityState.Modified;
                        if (!string.IsNullOrEmpty(ReturnForm.CustomerList))
                        {
                            cp.ZhanWeiLockCount -= (ReturnForm.AllCount - ReturnForm.CustomerList.Split('+').Count());
                            cp.ZhanWeiCount += (ReturnForm.AllCount - ReturnForm.CustomerList.Split('+').Count());
                            foreach (var Id in ReturnForm.CustomerList.Split(','))
                            {
                                var customerInfo = db.CustomerInfoes.Find(int.Parse(Id));
                                customerInfo.State = (int)EnumCustomer.ZhengChang;
                                db.Entry<CustomerInfo>(customerInfo).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        else
                        {
                            cp.ZhanWeiLockCount -= ReturnForm.AllCount;
                            cp.ZhanWeiCount += ReturnForm.AllCount;
                        }
                        db.HandleReturnForms.Add(
                            new HandleReturnForm
                            {
                                CreateTime = DateTime.Now,
                                Reason = note,
                                ReturnFormId = ReturnForm.Id,
                                UserId = LoginInfo.UserId
                            });
                        db.Messages.Add(new FenXiao.Model.Message
                        {
                            CreateTime = DateTime.Now,
                            IsRead = 0,
                            RelatedId = ReturnForm.Id,
                            State = (int)EnumMessage.chulituidan,
                            MessageContent = string.Format("退订单{0}，名称{1},数量{2}已被拒绝",
                            ReturnForm.Id, ReturnForm.Name, ReturnForm.AllCount),
                            ToCompanyId = ReturnForm.User.CompanyId
                        });
                        db.SaveChanges();
                    }
                }
                else
                {
                    return RedirectToAction("HandleByOther");
                }
                
            }
            return RedirectToAction("HandlingReturnOrderView");
        }

        public ActionResult HandleByOther()
        {
            return View();
        }
        /// <summary>
        /// 处理记录，展示哪个订单，哪个线路，是谁处理的
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnHistory(int id=0)
        {
            var HandleReturnForms = db.HandleReturnForms.OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(HandleReturnForms);
        }
    }
}