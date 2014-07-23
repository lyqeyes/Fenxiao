﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yeanzhi.Framework.Utility;

namespace FenXiao.Web.Extension
{
    public enum EnumRole
    {
        [EnumTitle("网站管理员", IsDisplay = false)]
        guanli = 0,
        [EnumTitle("网站子管理员", IsDisplay = false)]
        ziguanli = 1,
        [EnumTitle("批发商总管理", IsDisplay = false)]
        pifa = 2,
        [EnumTitle("批发商子管理", IsDisplay = false)]
        zipifa = 3,
        [EnumTitle("零售商总管理", IsDisplay = false)]
        lingshou = 4,
        [EnumTitle("零售商子管理", IsDisplay = false)]
        zilingshou = 5,
    }
}
