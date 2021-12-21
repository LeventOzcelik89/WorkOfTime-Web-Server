using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SV
{
    public class SVAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SV";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SV_default",
                "SV/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}