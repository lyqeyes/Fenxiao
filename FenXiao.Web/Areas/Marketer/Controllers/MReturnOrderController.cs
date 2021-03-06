﻿using FenXiao.Model;
using FenXiao.Web.Common;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FenXiao.Web.Areas.Marketer.Controllers
{
    public class MReturnOrderController : MarketerControllerBase
    {
        /// <summary>
        /// 待处理退订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HandlingReturnOrderView(int id = 0)
        {
            var order = db.ReturnForms.Where(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                a.State == (int)EnumReturnForm.xiatuidan).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(order);
        }
        [HttpGet]
        public ActionResult HandlingReturnOrderParialView(int orderid = 0)
        {
            var order = db.ReturnForms.FirstOrDefault(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                a.State == (int)EnumReturnForm.xiatuidan && a.Id == orderid);
            return View(order);
        }
        /// <summary>
        /// 已处理退订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HandledReturnOrderView(int id = 0)
        {
            var order = db.ReturnForms.Where(a =>
               a.User.CompanyId == LoginInfo.CompanyId &&
               a.State != (int)EnumReturnForm.xiatuidan).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(order);
        }
        [HttpGet]
        public ActionResult HandledReturnOrderParialView(int orderid = 0)
        {
            var order = db.ReturnForms.FirstOrDefault(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                a.State != (int)EnumReturnForm.xiatuidan && a.Id == orderid);
            return View(order);
        }
        /// <summary>
        /// 退单详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReturnOrderDetial(int id)
        {
            var reor = db.ReturnForms.Find(id);
            if (reor == null)
            {
                return HttpNotFound();
            }
            this.PermissionCompanyId = reor.User.CompanyId;          
            return View(reor);
        }

        /// <summary>
        /// 处理记录，展示哪个订单，哪个线路，是谁处理的
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReturnHistory(int id = 0)
        {
            var HandleReturnForms = db.HandleReturnForms.Where(a => a.ReturnForm.User.CompanyId == LoginInfo.CompanyId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(HandleReturnForms);
        }
        [HttpGet]
        public ActionResult ReturnHistoryParialView(int orderid = 0)
        {
            var handleReturnForm = db.HandleReturnForms.FirstOrDefault(a => a.ReturnForm.User.CompanyId == LoginInfo.CompanyId && a.Id == orderid);
            return View(handleReturnForm);
        }
    }
}