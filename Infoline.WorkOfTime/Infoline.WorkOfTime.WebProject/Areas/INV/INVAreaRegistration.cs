using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.INV
{
    public class INVAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "INV";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "INV_default",
                "INV/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}