using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.FVR
{
    public class FVRAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FVR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FVR_default",
                "FVR/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}