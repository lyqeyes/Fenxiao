using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    public enum EnumApply
    {
        [EnumTitle("正在申请", IsDisplay = false)]
        Applying = 0,
        [EnumTitle("通过申请", IsDisplay = false)]
        Success = 1,
        [EnumTitle("拒绝申请", IsDisplay = false)]
        False = 2,
    }
}
