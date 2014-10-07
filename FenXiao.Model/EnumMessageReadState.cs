using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    /// <summary>
    /// 消息阅读状态枚举
    /// </summary>
    public enum EnumMessageReadState
    {
        /// <summary>
        /// 已读
        /// </summary>
        [EnumTitle("已读", IsDisplay = false)]
        Read = 1,
        /// <summary>
        /// 未读
        /// </summary>
        [EnumTitle("未读", IsDisplay = false)]
        NotRead = 0
    }
}
