using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    /// <summary>
    /// 顾客状态枚举
    /// </summary>
    public enum EnumCustomer
    {
        /// <summary>
        /// 状态为正常
        /// </summary>
        [EnumTitle("正常", IsDisplay = false)]
        ZhengChang = 0,
        /// <summary>
        /// 状态为已删除
        /// </summary>
        [EnumTitle("已删除", IsDisplay = false)]
        YiShanChu = 1,
        /// <summary>
        /// 状态为待删除
        /// </summary>
        [EnumTitle("待删除", IsDisplay = false)]
        DaiShanChu = 2
    }
}
