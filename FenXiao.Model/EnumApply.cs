using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    /// <summary>
    /// 商户变更所属类别申请状态枚举
    /// </summary>
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
