using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Model
{
    /// <summary>
    /// 商户类型枚举
    /// </summary>
    public enum EnumCompany
    {
        [EnumTitle("零售商", IsDisplay = false)]
        lingshou = 0,
        [EnumTitle("批发商", IsDisplay = false)]
        pifa = 1,
        [EnumTitle("网站管理", IsDisplay = false)]
        guanli = 2,
        [EnumTitle("暂时的批发商", IsDisplay = false)]
        zanshipifa = 3,
        [EnumTitle("暂时的零售商", IsDisplay = false)]
        zanshilingshou = 4,
        [EnumTitle("注册成零售和批发", IsDisplay = false)]
        zhucelingshoupifa = 5,
        [EnumTitle("注册成零售", IsDisplay = false)]
        zhucelingshou = 6,
        [EnumTitle("注册成批发", IsDisplay = false)]
        zhucepifa = 7,
    }
}
