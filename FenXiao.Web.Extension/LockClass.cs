using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenXiao.Web.Extension
{
    //线程锁
    public class LockClass
    {
        public static object obj = new object();
        public static object objOrder = new object();
        public static object objApplyToLingShou = new object();
        public static object objApplyToPiFa = new object();
        public static object objDealApply = new object();
    }
}
