using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SYS.Controllers
{
    public class VWSYS_VersionController : Controller
    {
        [AllowEveryone]
        [PageInfo("Versiyon Yönetimi")]
        public ActionResult Index(VWSYS_VersionModel model)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            return View(model.Load());
        }

    }
}
