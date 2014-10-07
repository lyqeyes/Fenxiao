using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    /// <summary>
    /// 权限检查类别枚举
    /// </summary>
    public enum EnumAuthorityCheckType
    {
        /// <summary>
        /// 线路
        /// </summary>
        [EnumTitle("线路", IsDisplay = false)]
        Product,
        /// <summary>
        /// 子线路
        /// </summary>
        [EnumTitle("子线路", IsDisplay = false)]
        ChildProduct,
        /// <summary>
        /// 订单
        /// </summary>
        [EnumTitle("订单", IsDisplay = false)]
        OrderForm,
        /// <summary>
        /// 退单
        /// </summary>
        [EnumTitle("退单", IsDisplay = false)]
        ReturnForm,
        /// <summary>
        /// 申请
        /// </summary>
        [EnumTitle("申请", IsDisplay = false)]
        Apply,
        /// <summary>
        /// 微信营销
        /// </summary>
        [EnumTitle("微信营销", IsDisplay = false)]
        WeChat
    }
}
