using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    public enum EnumPageState
    {
        [EnumTitle("创建的营销页对系统内其他公司是公开", IsDisplay = false)]
        publicState = 0,
        [EnumTitle("创建的营销页对系统内其他公司是私有", IsDisplay = false)]
        privateState = 1,
    }

    public enum EnumPageType
    {
        [EnumTitle("只有一个案例", IsDisplay = false)]
        one = 1,
    }
}
