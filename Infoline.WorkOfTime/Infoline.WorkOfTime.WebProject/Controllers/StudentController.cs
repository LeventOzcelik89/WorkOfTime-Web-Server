using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Controllers
{
    public class StudentController : Controller
    {
        [PageInfo("Öğrenci Bilgi Sistemi", SHRoles.Ogrenci)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Öğrenci Verileri", SHRoles.Ogrenci)]
        public JsonResult GetAllDataByStudent()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var model = new FBU_OISModel(userStatus.user.email);
            var result = model.GetAllDataByStudent(userStatus.user.email);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Akademik Takvim", SHRoles.Ogrenci)]
        public ActionResult AcademicCalendar()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var model = new FBU_OISModel(userStatus.user.email);
            var res = model.GetAkademikTakvim();
            return View(res);
        }


        [PageInfo("Sınav Sonuçları", SHRoles.Ogrenci)]
        public ActionResult ResultsofExam()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var model = new FBU_OISModel(userStatus.user.email);
            var res = model.GetOgrenciSinavSonuc();
            return View(res);
        }

        [PageInfo("Transkript Talebi", SHRoles.Ogrenci)]
        public JsonResult RequestTranskript()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var model = new FBU_OISModel(userStatus.user.email);
            var res = model.SendTranskriptRequest();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}