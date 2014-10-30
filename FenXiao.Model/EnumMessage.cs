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
        [EnumTitle("下占位定单", IsDisplay = false)]
        ReserveNowOrder = 8,
        [EnumTitle("下直接报名定单", IsDisplay = false)]
        DirectApplyOrder = 9,
        [EnumTitle("下直接报名定单，正在编辑", IsDisplay = false)]
        DirectApplyOrderEditing = 10,
        [EnumTitle("公司编辑资料", IsDisplay = false)]
        CompanyEdit = 11,
        [EnumTitle("后补订单", IsDisplay = false)]
        HouBuOrder = 12
    }
}
