using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Controllers
{
    [AllowEveryone]
    public class ErrorController : Controller
    {
        public ActionResult UnkownPage(string aspxerrorpath)
        {
            if(!string.IsNullOrWhiteSpace(aspxerrorpath))
            {
                return RedirectToAction("UnkownPage");
            }
            return View();
        }

        public ActionResult InternalServer()
        {
            return View();
        }


        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}