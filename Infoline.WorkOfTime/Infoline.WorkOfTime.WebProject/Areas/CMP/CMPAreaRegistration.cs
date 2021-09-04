using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP
{
    public class CMPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CMP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CMP_default",
                "CMP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}