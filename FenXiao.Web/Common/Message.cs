using FenXiao.Model;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FenXiao.Web.Common
{
    public class Message
    {
        public static int GetWholesalerCount()
        {
            return DateSourceContext.Current.Messages.Where(a => a.ToCompanyId == FenXiaoUserContext.Current.LoginInfo.CompanyId
                && a.IsRead == 0 &&
                (a.State == (int)EnumMessage.xiadingdan ||
                a.State == (int)EnumMessage.xiatuidan ||
                a.State == (int)EnumMessage.chulilingshoushangshenqing)).
                Count();
        }

        public static int GetMarketerCount()
        {
            return DateSourceContext.Current.Messages.Where(a => a.ToCompanyId == FenXiaoUserContext.Current.LoginInfo.CompanyId
                && a.IsRead == 0 &&
                (a.State == (int)EnumMessage.chulidingdan
                || a.State == (int)EnumMessage.chulituidan
                || a.State == (int)EnumMessage.chulipifashangshenqing)).
                Count();
        }
    }
}