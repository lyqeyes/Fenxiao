
using System;
using System.Web;
namespace FenXiao.Web.Extension
{
   /// <summary>
    /// 线程锁
   /// </summary>
    public static class LockClass
    {
        public static object obj = new object();
        public static object objOrder = new object();
        public static object objApplyToLingShou = new object();
        public static object objApplyToPiFa = new object();
        public static object objDealApply = new object();

        #region 锁内部方法
        //各种内部锁
        private static object ProductLock = new object();
        private static object ChildProductLock = new object();
        private static object OrderFormLock = new object();
        private static object ReturnFormLock = new object();
        private static object ApplyLock = new object();
        //从缓存中获取锁
        private static object GetLock(string prefix, int Id)
        {
            object Temp = HttpRuntime.Cache.Get(prefix + Id.ToString());
            if (Temp != null)
            {
                return Temp;
            }
            else
            {
                Temp = new object();
                HttpRuntime.Cache.Insert(prefix + Id.ToString(), Temp, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(2));
                return Temp;
            }
        }
        #endregion

        #region 各种锁
        /// <summary>
        /// 线路锁
        /// </summary>
        /// <param name="productId">线路的Id</param>
        /// <returns></returns>
        public static object GetProductLock(int productId)
        {
            lock (ProductLock)
            {
                return GetLock("Product", productId);
            }
        }
        /// <summary>
        /// 子线路锁
        /// </summary>
        /// <param name="childProductId">子线路的Id</param>
        /// <returns></returns>
        public static object GetChildProductLock(int childProductId)
        {
            lock (ChildProductLock)
            {
                return GetLock("ChildProduct", childProductId);
            }
        }
        /// <summary>
        /// 订单锁
        /// </summary>
        /// <param name="orderFormId">订单的Id</param>
        /// <returns></returns>
        public static object GetOrderFormLock(int orderFormId)
        {
            lock (OrderFormLock)
            {
                return GetLock("OrderForm", orderFormId);
            }
        }
        /// <summary>
        /// 退单锁
        /// </summary>
        /// <param name="returnFormId"> 退单的Id</param>
        /// <returns></returns>
        public static object GetReturnFormLock(int returnFormId)
        {
            lock (ReturnFormLock)
            {
                return GetLock("ReturnForm", returnFormId);
            }
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="applyId">申请的Id</param>
        /// <returns></returns>
        public static object GetApplyLock(int applyId)
        {
            lock (ApplyLock)
            {
                return GetLock("Apply", applyId);
            }
        }
        #endregion
    }
}
