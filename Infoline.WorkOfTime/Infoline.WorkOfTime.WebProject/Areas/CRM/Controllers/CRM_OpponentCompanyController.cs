using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CRM.Controllers
{
	[PageInfo("Rakip Firma")]
    public class CRM_OpponentCompanyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [PageInfo("Rakip Firma Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetCRM_OpponentCompany(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetCRM_OpponentCompanyCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Rakip Firma Veri Methodu", SHRoles.Personel,SHRoles.BayiPersoneli,SHRoles.CagriMerkezi)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetCRM_OpponentCompany(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Rakip Firma Detayı", SHRoles.CRMYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetCRM_OpponentCompanyById(id);
            return View(data);
        }


        [PageInfo("Rakip Firma Ekleme", SHRoles.CRMYonetici)]
        public ActionResult Insert()
        {
            var data = new CRM_OpponentCompany { id = Guid.NewGuid() };
            return View(data);
        }

        [PageInfo("Rakip Firma Ekleme Methodu", SHRoles.CRMYonetici, SHRoles.SatisPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CRM_OpponentCompany item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var control = db.GetCRM_OpponentCompanyControl(item.CompanyName);

            if (control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Zaten aynı kayıt bulunmakta")
                }, JsonRequestBehavior.AllowGet);
            }
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertCRM_OpponentCompany(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Rakip Firma Güncelleme",SHRoles.CRMYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetCRM_OpponentCompanyById(id);
            return View(data);
        }

        [PageInfo("Rakip Firma Güncelleme Methodu", SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(CRM_OpponentCompany item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var control = db.GetCRM_OpponentCompanyUpdate(item.id, item.CompanyName);

            if (control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Kayıt Zaten Bulunmaktadır.")
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateCRM_OpponentCompany(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Rakip Firma Silme")]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new CRM_OpponentCompany { id = new Guid(a) });

            var dbresult = db.BulkDeleteCRM_OpponentCompany(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
