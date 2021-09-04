using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Controllers
{
    public class CustomerController : Controller
    {
        [PageInfo("Müşteri Bilgi Sistemi", SHRoles.SahaGorevMusteri)]
        public ActionResult Index()
        {
            return View();
        }


    }
}