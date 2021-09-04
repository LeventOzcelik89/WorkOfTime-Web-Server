using System.Web.Mvc;
                namespace Infoline.WorkOfTime.WebProject.Areas.PA
                {
                    public class PAAreaRegistration : AreaRegistration
                    {
                        public override string AreaName
                        {
                            get
                            {
                                return "PA";
                            }
                        }

                        public override void RegisterArea(AreaRegistrationContext context)
                        {
                            context.MapRoute(
                                "PA_default",
                                "PA/{controller}/{action}/{id}",
                                new { action = "Index", id = UrlParameter.Optional }
                            );
                        }
                    }
                }