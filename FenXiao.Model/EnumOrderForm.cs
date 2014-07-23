using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    /// <summary>
    /// 订单状态枚举
    /// </summary>
    public enum EnumOrderForm
    {
        [EnumTitle("下订单", IsDisplay = false)]
        xiadingdan = 0,
        [EnumTitle("处理订单", IsDisplay = false)]
        chulidingdan = 2,
    }

    /// <summary>
    /// 退单状态枚举
    /// </summary>
    public enum EnumReturnForm
    {
        [EnumTitle("下退单", IsDisplay = false)]
        xiatuidan = 0,
        [EnumTitle("取消退单", IsDisplay = false)]
        quxiaodingdan = 1,
        [EnumTitle("处理退单", IsDisplay = false)]
        chulidingdan = 2,
    }
}
