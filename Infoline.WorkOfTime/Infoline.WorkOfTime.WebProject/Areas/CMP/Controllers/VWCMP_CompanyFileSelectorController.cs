using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infoline.WorkOfTime.BusinessAccess.Business;
using Infoline.Framework.Database;
namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
    public class VWCMP_CompanyFileSelectorController : Controller
    {
        [PageInfo("Firma İzin Evraklarının Listelendiği Sayfa", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.HakEdisBayiPersoneli)]
        public ActionResult Index()
        {
            return View();
        }
        [AllowEveryone]
        [PageInfo("Firma İzin Evraklarının Listelendiği Metod", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.HakEdisBayiPersoneli)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_CompanyFileSelector(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCMP_CompanyFileSelectorCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Firma İzin Evraklarının Eklendiği Metod", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.HakEdisBayiPersoneli)]
        public ActionResult Insert(Guid customerId)
        {
            var data = new VMCMP_CompanyFileSelectorModel { id = Guid.NewGuid(), customerId = customerId };
            return View(data);
        }
        [PageInfo("Firma İzin Evraklarının Eklendiği   Metod", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.HakEdisBayiPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMCMP_CompanyFileSelectorModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Insert(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [PageInfo("Firma İzin Evraklarının Silindiği   Metod", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.HakEdisBayiPersoneli)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var dbresult = new VMCMP_CompanyFileSelectorModel { id = id }.Delete();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Ekleme Sayfası için Tüm datatabların çekildiği metod", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.HakEdisBayiPersoneli)]
        public JsonResult GetAllDataTableFromSysFile()
        {
            var data = new VMCMP_CompanyFileSelectorModel().GetAllDataTableFromSysFile();
            return Json(data.objects, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Firma İzin Evraklarının Silindiği   Metod", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli,SHRoles.HakEdisBayiPersoneli)]
        public JsonResult GetAllFileGroupNameFromSysFile([DataSourceRequest] DataSourceRequest request, Guid companyId)
        {
            var db = new WorkOfTimeDatabase();
            var existBefore = db.GetVWCMP_CompanyFileSelectorByCustomerId(companyId).ToList();
            List<SYS_Files> filebase = new List<SYS_Files>();
            var condition = KendoToExpression.Convert(request);
            condition.Filter.GetSerializeObject();
            foreach (var item in LocalFileProvider._dataTableFileGroup.Where(x => x.Key == condition.Filter.Operand2.ToString()))
            {
                var values = item.Value.Where(x => !existBefore.Where(c => c.fileGroupModule == item.Key).Select(a => a.fileGroupName).Contains(x.fileGroup));
                foreach (var value in values)
                {
                    filebase.Add(new SYS_Files { FileGroup = value.fileGroup, DataTable = item.Key });
                }
            }
            return Json(filebase, JsonRequestBehavior.AllowGet);
        }
    }
}
