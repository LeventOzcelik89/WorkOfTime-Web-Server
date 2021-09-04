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
    public class VWINV_PermitOfficalController : Controller
    {
        [PageInfo("Resmi İzin Tanımları", SHRoles.IKYonetici)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var model = db.GetVWINV_PermitOfficialSummary();
            return View(model);
        }

        [PageInfo("Resmi İzinler Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_PermitOffical(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_PermitOfficalCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Resmi İzinler Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_PermitOfficalById(id);
            return View(data);
        }


        [PageInfo("Resmi İzin Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert()
        {
            var data = new VWINV_PermitOffical { id = Guid.NewGuid() };
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Resmi İzin Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(INV_PermitOffical item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertINV_PermitOffical(item);
            INV_PermitCalculator.officalPermit = INV_PermitCalculator.checkOfficialPermit(new WorkOfTimeDatabase().GetINV_PermitOffical());
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Resmi İzinleri Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_PermitOfficalById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Resmi İzinleri Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(INV_PermitOffical item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateINV_PermitOffical(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Resmi İzinleri Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var item = id.Select(a => new INV_PermitOffical { id = new Guid(a) });
            var dbresult = db.BulkDeleteINV_PermitOffical(item);
            INV_PermitCalculator.officalPermit = INV_PermitCalculator.checkOfficialPermit(new WorkOfTimeDatabase().GetINV_PermitOffical());

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
