using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    public enum EnumCustomer
    {
        [EnumTitle("正常", IsDisplay = false)]
        zhengchang = 0,
        [EnumTitle("已删除", IsDisplay = false)]
        yishanchu = 1
    }
}
