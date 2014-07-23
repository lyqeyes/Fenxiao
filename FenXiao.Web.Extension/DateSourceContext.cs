using FenXiao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FenXiao.Web.Extension
{
    public class DateSourceContext
    {
        public static FenXiaoDBEntities Current
        {
            get { return HttpContext.Current.Items["_EntityContext"] as FenXiaoDBEntities; }
        }
    }
}
