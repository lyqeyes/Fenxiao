using FenXiao.Model;
using FenXiao.Web.Common;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;


namespace FenXiao.Web.Areas.Marketer.Controllers
{
    /// <summary>
    /// 订单处理记录，未处理订单，已处理订单（各类搜索）
    /// </summary>
    public class MOrderController : MarketerControllerBase
    {
        #region 占位订单部分
        [HttpGet]
        public ActionResult ReserveNowList(int index = 0)
        {
            var list =db.OrderForms.Where(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                a.State == (int)EnumOrderForm.ReserveNowOrder).OrderByDescending(a => a.Id).ToPagedList(index, PageSize);
            return View(list);
        }
        [HttpGet]
        public ActionResult ReserveNowParialView(int orderid = 0)
        {
            var order = db.OrderForms.FirstOrDefault(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                (a.State == (int)EnumOrderForm.ReserveNowOrder || a.State == (int)EnumOrderForm.DirectApplyOrderEditing || a.State == (int)EnumOrderForm.DirectApplyOrder) && a.Id == orderid);
            return View(order);
        }
        [HttpGet]
        public ActionResult ReserveNowDetail(int OrderFormId)
        {
            var orderForm = db.OrderForms.FirstOrDefault(e=>e.Id == OrderFormId);
            if (orderForm == null)
                return HttpNotFound();
            this.PermissionCompanyId = orderForm.User.CompanyId;
            return View(orderForm);
        }
        #endregion

        #region 直接订单部分
        [HttpGet]
        public ActionResult DirectApplyList(int index = 0)
        {
            var list = db.OrderForms.Where(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                (a.State == (int)EnumOrderForm.DirectApplyOrderEditing || a.State == (int)EnumOrderForm.DirectApplyOrder)).OrderByDescending(a => a.Id).ToPagedList(index, PageSize);
            return View(list);
        }
        [HttpGet]
        public ActionResult DirectApplyParialView(int orderid = 0)
        {
            var order = db.OrderForms.FirstOrDefault(a =>
                a.User.CompanyId == LoginInfo.CompanyId &&
                (a.State == (int)EnumOrderForm.ReserveNowOrder || a.State == (int)EnumOrderForm.DirectApplyOrderEditing || a.State == (int)EnumOrderForm.DirectApplyOrder) && a.Id == orderid);
            return View(order);
        }
        [HttpGet]
        public ActionResult DirectApplyDetail(int OrderFormId)
        {
            var orderForm = db.OrderForms.FirstOrDefault(e => e.Id == OrderFormId);
            if (orderForm == null)
                return HttpNotFound();
             this.PermissionCompanyId = orderForm.User.CompanyId;
            return View(orderForm);
        }
        #endregion
    }
}