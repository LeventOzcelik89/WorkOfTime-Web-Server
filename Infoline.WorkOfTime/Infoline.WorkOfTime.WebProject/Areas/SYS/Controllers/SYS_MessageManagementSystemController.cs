using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SYS.Controllers
{
    public class SYS_MessageManagementSystemController : Controller
    {
        [PageInfo("İleti Yönetim Sistemi Anasayfası",SHRoles.SistemYonetici)]
        public ActionResult Index()
        {
            return View();
        }
    }
}