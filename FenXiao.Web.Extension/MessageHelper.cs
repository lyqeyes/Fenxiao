using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FenXiao.Model;
namespace FenXiao.Web.Extension
{
    /// <summary>
    /// 消息帮助类
    /// </summary>
    public static class MessageHelper
    {     
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="relatedId">与消息有关的Id</param>
        /// <param name="toCompanyId">向哪个公司Id</param>
        /// <param name="content">消息内容</param>
        /// <param name="state">消息类别</param>
        public static bool Add(int relatedId,int toCompanyId,  string content, EnumMessage state)
        {
            Message message = new Message();
            message.CreateTime = DateTime.Now;
            message.IsRead = 0;
            message.State = (int)state;
            message.RelatedId = relatedId;
            message.MessageContent = content;
            message.ToCompanyId = toCompanyId;
            DateSourceContext.Current.Entry<Message>(message).State = System.Data.Entity.EntityState.Added;
            if (DateSourceContext.Current.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="messageId">消息的主键Id</param>
        /// <returns></returns>
        public static bool Del(int messageId)
        {
            var message = DateSourceContext.Current.Messages.Find(messageId);
            if(message == null)
                return false;
            DateSourceContext.Current.Entry<Message>(message).State = System.Data.Entity.EntityState.Deleted;
            if (DateSourceContext.Current.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 消息标记已读
        /// </summary>
        public static bool IsRead(int messageId)
        {
            var message = DateSourceContext.Current.Messages.Find(messageId);
            if (message == null)
                return false;
            message.IsRead = 1;
            DateSourceContext.Current.Entry<Message>(message).State = System.Data.Entity.EntityState.Modified;
            if (DateSourceContext.Current.SaveChanges() > 0)
                return true;
            else
                return false;
        }
    }
}