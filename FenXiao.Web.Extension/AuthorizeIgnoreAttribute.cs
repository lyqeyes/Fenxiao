using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenXiao.Web.Extension
{
    /// <summary>
    /// 忽略权限属性
    /// </summary>
    public class AuthorizeIgnoreAttribute : Attribute
    {
        public AuthorizeIgnoreAttribute()
        {
        }
    }
}
