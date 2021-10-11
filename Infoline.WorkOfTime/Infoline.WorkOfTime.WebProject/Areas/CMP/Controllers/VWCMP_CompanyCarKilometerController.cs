using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
    public class VWCMP_CompanyCarKilometerController : Controller
    {
        [PageInfo("Araç Kilometre Bilgilerim", SHRoles.Personel)]
        public ActionResult Index()
        {
            return View();
        }
        [PageInfo("Araç Kilometre Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_CompanyCarKilometer(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCMP_CompanyCarKilometerCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Araç Kilometre Dropdown Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_CompanyCarKilometer(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Araç Kilometre Tanımla", SHRoles.Personel)]
        public ActionResult Insert(Guid? id, Guid? companyCarId)
        {
            var data = new VWCMP_CompanyCarKilometer();

            if (id.HasValue)
            {
                data.companyCarId = companyCarId;
                data.id = Guid.NewGuid();
                if (!companyCarId.HasValue)
                {
                    data.companyCarId = id;
                }
                return View(data);
            }
            data.id = Guid.NewGuid();

            return View(data);
        }

        [PageInfo("Araç Kilometre Tanımla", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CMP_CompanyCarKilometer item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.id = Guid.NewGuid();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var kilometerCheckMax = db.GetCMP_CompanyCarKilometerByMaxKilometer(item.companyCarId.Value, item.entryDate.Value);
            if (kilometerCheckMax != null && (float)item.kilometer.Value < (float)kilometerCheckMax.kilometer.Value)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Bir önceki tarihteki kilometre değerinden daha büyük bir değer girmelisiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            if (kilometerCheckMax!=null && (float)item.kilometer.Value == (float)kilometerCheckMax.kilometer.Value)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Bir önceki tarihteki kilometre değeriyle aynı olamaz.")
                }, JsonRequestBehavior.AllowGet);
            }
            //var kilometerCheckMin = db.GetCMP_CompanyCarKilometerByMinKilometer(item.companyCarId.Value, item.entryDate.Value);
            //if (kilometerCheckMin != null && (float)item.kilometer.Value >= (float)kilometerCheckMin.kilometer.Value)
            //{
            //    return Json(new ResultStatusUI
            //    {
            //        Result = false,
            //        FeedBack = feedback.Warning("Bir sonraki tarihteki kilometre değerinden daha küçük bir değer girmelisiniz.")
            //    }, JsonRequestBehavior.AllowGet);
            //}

            var dbresult = db.InsertCMP_CompanyCarKilometer(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Görevlendirmeye Araç Kilometre Tanımla", SHRoles.Personel)]
        [AllowEveryone]
        public ActionResult CommissionsInsert(Guid? id, Guid? companyCarId)
        {
            var data = new VWCMP_CompanyCarKilometer();
            var db = new WorkOfTimeDatabase();
            if (id.HasValue)
            {   
                var lastKilometer = db.GetCMP_CompanyCarKilometerByMaxStartKm(companyCarId.Value);
                data.kilometer = lastKilometer.kilometer;
                data.companyCarId = companyCarId;
                data.id = Guid.NewGuid();
                if (!companyCarId.HasValue)
                {
                    data.companyCarId = id;
                }
                return View(data);
            }
            data.id = Guid.NewGuid();

            return View(data);
        }

        [PageInfo("Görevlendirmeye Araç Kilometre Tanımla", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken, AllowEveryone]
        public JsonResult CommissionsInsert(CMP_CompanyCarKilometer item, bool? isPost, double? StartKm, double? EndKm)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.id = Guid.NewGuid();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var kilometerCheckMax = db.GetCMP_CompanyCarKilometerByMaxStartKm(item.companyCarId.Value);
            if (kilometerCheckMax != null && StartKm < kilometerCheckMax.kilometer.Value)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Bir önceki tarihteki kilometre değerinden daha büyük bir değer girmelisiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            if (kilometerCheckMax != null && EndKm == kilometerCheckMax.kilometer.Value)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Bir önceki tarihteki kilometre değeriyle aynı olamaz.")
                }, JsonRequestBehavior.AllowGet);
            }

            if (kilometerCheckMax != null)
            {
                var dbKM = db.GetCMP_CompanyCarKilometerById(kilometerCheckMax.id);
                var totalKm = EndKm - StartKm;
                item.kilometer = dbKM.kilometer + totalKm;
            }

            var dbresult = db.InsertCMP_CompanyCarKilometer(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
