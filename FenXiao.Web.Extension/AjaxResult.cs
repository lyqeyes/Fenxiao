using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenXiao.Web.Extension
{
    /// <summary>
    /// Ajax结果类
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 返回的信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 请求是否成功提交
        /// </summary>
        public int Ok { get; set; }
    }
}
