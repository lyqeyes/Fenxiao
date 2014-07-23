using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FenXiao.Web.Extension
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class ControllerBase : Controller
    {
        /// <summary>
        /// log4net日志记录
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Loggering");

        /// <summary>
        /// 默认分页大小
        /// </summary>
        public virtual int PageSize
        {
            get
            {
                return 25;
            }
        }


        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var e = filterContext.Exception;
        }

        protected virtual void LogException(Exception exception, WebExceptionContext exceptionContext = null)
        {
        }

        protected virtual ActionResult HttpNotPermission()
        {
            return RedirectToAction("NoPermission","Home");
        }
    }

    /// <summary>
    /// Web异常信息类
    /// </summary>
    public class WebExceptionContext
    {
        public string IP { get; set; }
        public string CurrentUrl { get; set; }
        public string RefUrl { get; set; }
        public bool IsAjaxRequest { get; set; }
        public NameValueCollection FormData { get; set; }
        public NameValueCollection QueryData { get; set; }
    }
}
