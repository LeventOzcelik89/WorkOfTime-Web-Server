using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ
{
    public class PRJAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PRJ";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PRJ_default",
                "PRJ/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}