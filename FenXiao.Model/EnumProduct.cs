using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    public enum EnumProduct
    {
        [EnumTitle("正常", IsDisplay = false)]
        zhengchang = 1,
        [EnumTitle("禁用", IsDisplay = false)]
        jinyong = 0,
    }
}
