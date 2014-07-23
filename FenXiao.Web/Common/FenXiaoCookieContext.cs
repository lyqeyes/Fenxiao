using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Common
{
    public class FenXiaoCookieContext : CookieContext
    {
        public static FenXiaoCookieContext Current
        {
            get
            {
                return new FenXiaoCookieContext();
            }
        }

        public override string KeyPrefix
        {
            get
            {
                return "FenXiao" + "_Context_";
            }
        }
    }
}