using FenXiao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FenXiao.Web.Extension
{
    /// <summary>
    /// 数据上下文
    /// </summary>
    public class DateSourceContext
    {
        /// <summary>
        /// 当前的数据上下文
        /// </summary>
        public static FenXiaoDBEntities Current
        {
            get { return HttpContext.Current.Items["_EntityContext"] as FenXiaoDBEntities; }
        }
    }
}
