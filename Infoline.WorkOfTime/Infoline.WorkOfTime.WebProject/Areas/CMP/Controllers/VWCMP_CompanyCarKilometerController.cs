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

        [PageInfo("Araç Kilometre Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_CompanyCarKilometerById(id);
            return View(data);
        }

        [PageInfo("Araç Kilometre Tanımla", SHRoles.Personel)]
        public ActionResult Insert(Guid? id, Guid? companyCarId)
        {
            var db = new WorkOfTimeDatabase();
            var data = new VWCMP_CompanyCarKilometer();
            var userStatus = (PageSecurity)Session["userStatus"];

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
            var userCar = db.GetCMP_CompanyCarsByUserId(userStatus.user.id);
            if (userCar != null)
            {
                data.companyCarId = userCar.id;
            }

            return View(data);
        }

        [PageInfo("Araç Kilometre Tanımla", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CMP_CompanyCarKilometer item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            //var res = new FileUploadSave(Request).SaveAs();

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
            if (kilometerCheckMax != null && (float)item.kilometer.Value == (float)kilometerCheckMax.kilometer.Value)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Bir önceki tarihteki kilometre değeriyle aynı olamaz.")
                }, JsonRequestBehavior.AllowGet);
            }

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
        public ActionResult CommissionsInsert(Guid? id, Guid? companyCarId, Guid? commissionId)
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
            data.commissionId = commissionId;
            return View(data);
        }

        [PageInfo("Görevlendirmeye Araç Kilometre Tanımla", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken, AllowEveryone]
        public JsonResult CommissionsInsert(CMP_CompanyCarKilometer item, bool? isPost, double? StartKm, double? EndKm)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            //var res = new FileUploadSave(Request).SaveAs();

            item.id = Guid.NewGuid();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;


            var commission = db.GetINV_CommissionsById(item.commissionId.Value);


            if (StartKm == null || EndKm == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Başlangıç veya Bitiş kilometresini giriniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            if (EndKm <= StartKm)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Bitiş kilometresi başlangıç kilometresinden büyük olamaz.")
                }, JsonRequestBehavior.AllowGet);
            }

            var Start = new CMP_CompanyCarKilometer
            {
                created = DateTime.Now,
                createdby = userStatus.user.id,
                kilometer = StartKm,
                companyCarId = item.companyCarId,
                entryDate = commission.StartDate,
            };

            var End = new CMP_CompanyCarKilometer
            {
                created = DateTime.Now,
                createdby = userStatus.user.id,
                kilometer = EndKm,
                companyCarId = item.companyCarId,
                entryDate = DateTime.Now
            };

            commission.VehicleKilometer = EndKm - StartKm;

            var trans = db.BeginTransaction();

            var dbresult = db.InsertCMP_CompanyCarKilometer(Start, trans);
            dbresult &= db.InsertCMP_CompanyCarKilometer(End, trans);
            dbresult &= db.UpdateINV_Commissions(commission, true, trans);

            if (dbresult.result)
            { 
                trans.Commit();
            }
            else
            { 
                trans.Rollback();
            }
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}
