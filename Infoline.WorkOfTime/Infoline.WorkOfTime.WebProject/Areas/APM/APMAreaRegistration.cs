using System.Web.Mvc;
                namespace Infoline.WorkOfTime.WebProject.Areas.APM
                {
                    public class APMAreaRegistration : AreaRegistration
                    {
                        public override string AreaName
                        {
                            get
                            {
                                return "APM";
                            }
                        }

                        public override void RegisterArea(AreaRegistrationContext context)
                        {
                            context.MapRoute(
                                "APM_default",
                                "APM/{controller}/{action}/{id}",
                                new { action = "Index", id = UrlParameter.Optional }
                            );
                        }
                    }
                }