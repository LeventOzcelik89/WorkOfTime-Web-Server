using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
    public class INV_PermitTypeController : Controller
    {
        [PageInfo("İzin Tipi Tanımları",SHRoles.IKYonetici)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var model = db.GetVWINV_PermitTypeSummary();
            return View(model);
        }

        [PageInfo("İzin Tipleri Methodu", SHRoles.IKYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_PermitType(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_PermitTypeCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("İzin Tipleri Veri Methodu",SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var condition = KendoToExpression.Convert(request);
            var result = db.GetVWINV_PermitType(condition);
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.IKYonetici)))
            {
                result = result.Where(x => x.Name != "Sağlık Raporu / Vizite İzni").ToArray();
            }
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }


        [PageInfo("İzin Tipleri detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetINV_PermitTypeById(id);
            return View(data);
        }


        [PageInfo("İzin Tipleri Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert()
        {
            var data = new INV_PermitType { id = Guid.NewGuid() };
            return View(data);
        }


        [PageInfo("İzin Tipleri Ekleme", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(INV_PermitType item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertINV_PermitType(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("İzin Tipleri Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetINV_PermitTypeById(id);
            return View(data);
        }


        [PageInfo("İzin Tipleri Güncelleme", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(INV_PermitType item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var permitType = db.GetINV_PermitTypeById(item.id);

            permitType.changed = DateTime.Now;
            permitType.changedby = userStatus.user.id;

            permitType.Name = item.Name;
            permitType.BeDocumented = item.BeDocumented;
            permitType.CanHourly = item.CanHourly;
            permitType.Descriptions = item.Descriptions;
            permitType.IsPaidPermit = item.IsPaidPermit;
            permitType.PaidPermitDay = item.PaidPermitDay;
            permitType.RequestStaff = item.RequestStaff;
            permitType.ViewStaff = item.ViewStaff;

            var dbresult = db.UpdateINV_PermitType(permitType, true);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Düzenleme işlemi başarılı") : feedback.Error("Düzenleme işleminiz başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
