using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    public enum EnumUser
    {
        [EnumTitle("冻结", IsDisplay = false)]
        dongjie = 0,
        [EnumTitle("正常", IsDisplay = false)]
        zhengchang = 1,
    }
}
