using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    //TODO: 这是什么？
    public enum EnumMessageState
    {
        [EnumTitle("通过", IsDisplay = false)]
        tongguo = 1,
        [EnumTitle("拒绝", IsDisplay = false)]
        jujue = 0,
    }
}
