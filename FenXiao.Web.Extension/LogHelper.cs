using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenXiao.Web.Extension
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    // TODO: 未实现
    public class LogHelper
    {
        /// <summary>
        /// 添加消息
        /// </summary>
        public bool Add()
        {

            if (DateSourceContext.Current.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="messageId">消息的主键Id</param>
        public bool Del(int LogId)
        {
           if (DateSourceContext.Current.SaveChanges() > 0)
                return true;
            else
                return false;
        }
    }
}
