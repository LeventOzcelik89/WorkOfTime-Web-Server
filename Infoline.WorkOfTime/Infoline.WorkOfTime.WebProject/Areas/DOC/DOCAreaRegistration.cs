using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.DOC
{
    public class DOCAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DOC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DOC_default",
                "DOC/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}