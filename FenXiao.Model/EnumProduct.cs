using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    /// <summary>
    /// 产品线路状态枚举
    /// </summary>
    public enum EnumProduct
    {
        [EnumTitle("正常", IsDisplay = false)]
        zhengchang = 1,
        [EnumTitle("禁用", IsDisplay = false)]
        jinyong = 0,
    }
}
