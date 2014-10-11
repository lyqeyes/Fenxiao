using FenXiao.Model;
using FenXiao.Web.Extension;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FenXiao.Web.Common
{
    /// <summary>
    /// 营销商根类
    /// </summary>
    public class MarketerControllerBase : FenXiao.Web.Extension.ControllerBase
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

        //消息帮助类的上下文
        public MessageHelper MessageContext
        {
            get
            {
                return new MessageHelper();
            }
        }
        //日志帮助类的上下文
        public LogHelper LogContext
        {
            get
            {
                return new LogHelper();
            }
        }

        public int PermissionCompanyId
        {
            get
            {
                if (HttpContext.Items["CompanyId"] != null)
                {
                    return (int)HttpContext.Items["CompanyId"];
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                HttpContext.Items["CompanyId"] = value;
            }
        }

        public new ActionResult HttpNotFound()
        {
            return Redirect("~/Marketer/MError/HttpNotFound");
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = Redirect("~/Marketer/MError/BadRequestt");
            base.OnException(filterContext);
            filterContext.ExceptionHandled = true;
            //
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            if (this.PermissionCompanyId != -1)
            {
                if (this.LoginInfo.CompanyId != this.PermissionCompanyId)
                {
                    filterContext.Result = RedirectToAction("NoPermission", "MError",
                        new { Area = "Marketer" });
                    return;
                }
            }

            base.OnActionExecuted(filterContext);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            var noAuthorizeAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeIgnoreAttribute), false);
            if (noAuthorizeAttributes.Length > 0)
                return;

            

            if (this.LoginInfo == null)
            {
                filterContext.Result = RedirectToAction("Login", "MAuth", new { Area = "Marketer" });
                return;
            }
            if (this.LoginInfo.Companytype!=(int)EnumCompany.lingshou)
            {
                filterContext.Result = RedirectToAction("Login", "MAuth", new { Area = "Marketer" });
                return;
            }
            if (LoginInfo.Role.Count == 0)
            {
                RedirectToAction("NoPermission", "MError",
                        new { Area = "Marketer" });
            }
            else
            {
                if (!(LoginInfo.Role.Contains((int)EnumRole.lingshou) || LoginInfo.Role.Contains((int)EnumRole.zilingshou)))
                {
                    RedirectToAction("NoPermission", "MError",
                        new { Area = "Marketer" });
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}