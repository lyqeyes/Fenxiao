using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    //
    public enum EnumMessage
    {
        [EnumTitle("下订单", IsDisplay = false)]
        xiadingdan = 0,
        [EnumTitle("下退单", IsDisplay = false)]
        xiatuidan = 1,
        [EnumTitle("处理订单", IsDisplay = false)]
        chulidingdan = 2,
        [EnumTitle("处理退货单", IsDisplay = false)]
        chulituidan = 3,
        [EnumTitle("公司申请", IsDisplay = false)]
        shenqingpifashang = 4,
        [EnumTitle("公司申请", IsDisplay = false)]
        shenqinglingshoushang = 5,
        [EnumTitle("处理公司申请", IsDisplay = false)]
        chulilingshoushangshenqing = 6,
        [EnumTitle("处理公司申请", IsDisplay = false)]
        chulipifashangshenqing = 7,
    }
}
