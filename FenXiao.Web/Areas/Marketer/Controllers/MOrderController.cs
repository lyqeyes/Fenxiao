using FenXiao.Model;
using FenXiao.Web.Common;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;


namespace FenXiao.Web.Areas.Marketer.Controllers
{
    /// <summary>
    /// 订单处理记录，未处理订单，已处理订单（各类搜索）
    /// </summary>
    public class MOrderController : MarketerControllerBase
    {

        #region 订单管理部分

        /// <summary>
        /// 待处理订单
        /// </summary>
        /// <returns></returns>
        public ActionResult HandlingOrderView(int id=0)
        {
            
            var order = db.OrderForms.Where(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                a.State == (int)EnumOrderForm.xiadingdan).OrderByDescending(a=>a.Id).ToPagedList(id, PageSize);
            return View(order);
        }

        /// <summary>
        /// 已处理订单
        /// </summary>
        /// <returns></returns>
        public ActionResult HandledOrderView(int id = 0)
        {
            var order = db.OrderForms.Where(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                a.State == (int)EnumOrderForm.chulidingdan).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(order);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderDetial(int id)
        {
            var order = db.OrderForms.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        /// <summary>
        /// 处理记录
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderHistory(int id=0)
        {
            var HandleOrderForms = db.HandleOrderForms.Where(a => a.OrderForm.User.CompanyId == LoginInfo.CompanyId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(HandleOrderForms);
        }

        #endregion

        #region 退单管理部分

        /// <summary>
        /// 待处理退订单
        /// </summary>
        /// <returns></returns>
        public ActionResult HandlingReturnOrderView(int id = 0)
        {
            var order = db.ReturnForms.Where(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                a.State == (int)EnumReturnForm.xiatuidan).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(order);
        }

        /// <summary>
        /// 已处理退订单
        /// </summary>
        /// <returns></returns>
        public ActionResult HandledReturnOrderView(int id = 0)
        {
            var order = db.ReturnForms.Where(a =>
               a.User.CompanyId == LoginInfo.CompanyId &&
               a.State != (int)EnumReturnForm.xiatuidan).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(order);
        }

        /// <summary>
        /// 退单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnOrderDetial(int id)
        {
            var reor = db.ReturnForms.Find(id);
            if (reor == null)
            {
                return HttpNotFound();
            }
            return View(reor);
        }

        /// <summary>
        /// 处理记录，展示哪个订单，哪个线路，是谁处理的
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnHistory(int id=0)
        {
            var HandleReturnForms = db.HandleReturnForms.Where(a => a.ReturnForm.User.CompanyId == LoginInfo.CompanyId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(HandleReturnForms);
        }
        #endregion
    }
}