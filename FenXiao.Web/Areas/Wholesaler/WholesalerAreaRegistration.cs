using System.Web.Mvc;

namespace FenXiao.Web.Areas.Wholesaler
{
    public class WholesalerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Wholesaler";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Wholesaler_default",
                "Wholesaler/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}