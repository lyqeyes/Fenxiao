using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FenXiao.Model;
namespace FenXiao.Web.Extension
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 添加消息
        /// </summary>
        public bool Add(string Msg, EnumLogType LogType, int ? RelatedId)
        {
            var log = new Log();
            log.CreateTime = DateTime.Now;
            log.Msg = Msg;
            log.Type = (int)LogType;
            log.RelatedId = RelatedId;
            DateSourceContext.Current.Entry<Log>(log).State = System.Data.Entity.EntityState.Added;
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
            var log = DateSourceContext.Current.Logs.Find(LogId);
            if (log == null)
                return false;
            DateSourceContext.Current.Entry<Log>(log).State = System.Data.Entity.EntityState.Deleted;
            if (DateSourceContext.Current.SaveChanges() > 0)
                return true;
            else
                return false;
        }
    }
}