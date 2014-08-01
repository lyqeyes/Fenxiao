using FenXiao.Model;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Common
{
    /// <summary>
    /// 批发商根类
    /// </summary>
    public class WholesalerControllerBase : FenXiao.Web.Extension.ControllerBase
    {
        public FenXiaoDBEntities db
        {
            get
            {
                return DateSourceContext.Current;
            }
        }


        public FenXiaoCookieContext CookieContext
        {
            get
            {
                return FenXiaoCookieContext.Current;
            }
        }

        public FenXiaoUserContext UserContext
        {
            get
            {
                return FenXiaoUserContext.Current;
            }
        }

        public virtual UserLoginInfo LoginInfo
        {
            get
            {
                return UserContext.LoginInfo;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var noAuthorizeAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeIgnoreAttribute), false);
            if (noAuthorizeAttributes.Length > 0)
                return;

           

            if (this.LoginInfo == null)
            {
                filterContext.Result = RedirectToAction("Login", "WAuth", new { Area = "Wholesaler" });
                return;
            }
            if (this.LoginInfo.Companytype != (int)EnumCompany.pifa)
            {
                filterContext.Result = RedirectToAction("Login", "MAuth", new { Area = "Marketer" });
                return;
            }
            if (LoginInfo.Role.Count==0)
            {
                filterContext.Result = Content("没有权限！");
            }
            else
            {
                if (!(LoginInfo.Role.Contains((int)EnumRole.pifa)||LoginInfo.Role.Contains((int)EnumRole.zipifa)))
                {
                    filterContext.Result = Content("没有权限！");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}