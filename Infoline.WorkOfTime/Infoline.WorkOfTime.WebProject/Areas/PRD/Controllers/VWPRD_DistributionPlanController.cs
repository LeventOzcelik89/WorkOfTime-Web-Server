using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_DistributionPlanController : Controller
    {
        [PageInfo("Dağıtım/Sevkiyat Planı Listesi", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
        public ActionResult Index()
        {
            return View();
        }
        [PageInfo("Dağıtım/Sevkiyat Planı Grid DataSource", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli, SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_DistributionPlan(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_DistributionPlanCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Dağıtım/Sevkiyat Planı İlişkileri Grid DataSource", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli, SHRoles.IKYonetici)]
        public ContentResult DataSourcePlanRelation([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_DistributionPlanRelation(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_DistributionPlanRelationCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Dağıtım/Sevkiyat Planı Miktar DataSource", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            return db.GetVWPRD_DistributionPlanCount(condition.Filter);
        }

        [PageInfo("Dağıtım/Sevkiyat Planı Dropdown DataSource", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_DistributionPlan(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Dağıtım/Sevkiyat Planı Detayı", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.IKYonetici, SHRoles.UretimPersonel, SHRoles.UretimYonetici, SHRoles.Personel)]
        public ActionResult Detail(VMPRD_DistributionPlanModel model)
        {
            var data = model.Load();
            return View(data);
        }
       

        [PageInfo("Dağıtım/Sevkiyat Planı İşlem Girişi", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.IKYonetici)]
        public ActionResult Upsert(VMPRD_DistributionPlanModel model)
        {
            var data = model.Load();
            return View(data);
        }

        [PageInfo("Dağıtım/Sevkiyat Planı Ekleme ve Güncelleme", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Upsert(VMPRD_DistributionPlanModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Dağıtım/Sevkiyat Oluşturma İşlemi Başarılı") : feedback.Warning("Dağıtım/Sevkiyat Oluşturma İşlemi Başarısız")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Dağıtım/Sevkiyat Planı İşlem Yazdır", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi), ExportPDF]
        public ActionResult Print(VMPRD_TransactionModel model)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            model.created = model.created ?? DateTime.Now;
            model.createdby = model.createdby ?? userStatus.user.id;
            model.type = model.type ?? (int)EnumPRD_TransactionType.GelenIrsaliye;
            model.type_Title = model.type_Title ?? Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumPRD_TransactionType>().Where(a => Convert.ToInt16(a.Key) == model.type).Select(a => a.Value).FirstOrDefault();
            var data = model.Load();

            return PartialView("~/Areas/PRD/Views/VWPRD_Transaction/Print/" + (((TenantConfig.Tenant.TenantCode == 1999 || TenantConfig.Tenant.TenantCode == 1139) && (model.type == (int)EnumPRD_TransactionType.ZimmetAlma || model.type == (int)EnumPRD_TransactionType.ZimmetVerme)) ? TenantConfig.Tenant.TenantCode.ToString() + "/" : "") + "Action" + data.type + ".cshtml", data);
        }

        [HttpPost]
        [PageInfo("Dağıtım/Sevkiyat Planı İşlemi Sil", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
        public JsonResult Delete(Guid[] id)
        {

            var feedback = new FeedBack();
            var dbresult = new ResultStatus { result = true };
            foreach (var idmy in id)
            {
                dbresult &= new VMPRD_TransactionModel { id = idmy }.Delete();
            }

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarısız.Mesaj:İşlem başka işlemlerle bağlantı olabilir.")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

