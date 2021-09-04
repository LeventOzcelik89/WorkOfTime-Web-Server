using System.Web.Mvc;
                namespace Infoline.WorkOfTime.WebProject.Areas.UT
                {
                    public class UTAreaRegistration : AreaRegistration
                    {
                        public override string AreaName
                        {
                            get
                            {
                                return "UT";
                            }
                        }

                        public override void RegisterArea(AreaRegistrationContext context)
                        {
                            context.MapRoute(
                                "UT_default",
                                "UT/{controller}/{action}/{id}",
                                new { action = "Index", id = UrlParameter.Optional }
                            );
                        }
                    }
                }