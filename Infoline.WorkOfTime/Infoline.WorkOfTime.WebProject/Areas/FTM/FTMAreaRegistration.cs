using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.FTM
{
    public class FTMAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FTM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FTM_default",
                "FTM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}