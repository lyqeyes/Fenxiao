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
            var HandleOrderForms = db.HandleOrderForms.Where(a => a.OrderForm.User.CompanyId == LoginInfo.CompanyId).OrderByDescending(a => a.Id).ToPagedList(id, PageSize);
            return View(HandleOrderForms);
        }

        #endregion


    }
}