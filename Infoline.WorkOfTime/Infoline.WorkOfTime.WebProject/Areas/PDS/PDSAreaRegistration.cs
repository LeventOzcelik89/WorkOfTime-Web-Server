using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PDS
{
    public class PDSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PDS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PDS_default",
                "PDS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}