using System.Web.Mvc;
                namespace Infoline.WorkOfTime.WebProject.Areas.CSM
                {
                    public class CSMAreaRegistration : AreaRegistration
                    {
                        public override string AreaName
                        {
                            get
                            {
                                return "CSM";
                            }
                        }

                        public override void RegisterArea(AreaRegistrationContext context)
                        {
                            context.MapRoute(
                                "CSM_default",
                                "CSM/{controller}/{action}/{id}",
                                new { action = "Index", id = UrlParameter.Optional }
                            );
                        }
                    }
                }