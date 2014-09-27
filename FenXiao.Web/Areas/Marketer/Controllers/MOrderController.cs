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
            //var HandleOrderForms = db.HandleOrderForms.Where(a => a.OrderForm.User.CompanyId == LoginInfo.CompanyId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            //return View(HandleOrderForms);
            return View();
        }

        #endregion

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
        public ActionResult ReserveNowDetail(int OrderFormId)
        {
            var orderForm = db.OrderForms.FirstOrDefault(e=>e.Id == OrderFormId);
            if (orderForm == null)
                return HttpNotFound();
            return View(orderForm);
        }
        #endregion

        #region 已付款直接订单
        #endregion

        #region 未付款直接订单
        #endregion

        #region 处理记录部分
        #endregion
    }
}