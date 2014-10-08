using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum EnumLogType
    {
        [EnumTitle("系统日志", IsDisplay = false)]
        System = 0,
        [EnumTitle("用户日志", IsDisplay = false)]
        User = 1
    }
}
