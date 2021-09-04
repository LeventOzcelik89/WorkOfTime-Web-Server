using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
	public class VWFTM_TaskFormResultController : Controller
    {
        [PageInfo("Saha Görev Formları", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWFTM_TaskFormResult(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWFTM_TaskFormResultCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Saha Görev Formu Doldur")]
        public ActionResult InsertMobile(VMFTM_TaskOperationModel request)
        {
            request.status = (int)EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAltUrun;
            var model = request.Load();
            return View(model);
        }

        [AllowEveryone]
        [PageInfo("Saha Görev Formu Ekle (Mobil)", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult InsertMobile(VMFTM_TaskOperationModel item, bool? isPost)
        {
            var feedback = new FeedBack();
            item.status = (int)EnumFTM_TaskOperationStatus.GorevFormuDoldurulduAnaUrun;
            var dbresult = item.Insert(item.createdby);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message, false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning(dbresult.message),
            }, JsonRequestBehavior.AllowGet);
        }



        [AllowEveryone]
        [PageInfo("Saha Görev Formu Düzenle (Mobil)")]
        public ActionResult UpdateMobile(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var model = db.GetVWFTM_TaskFormResultById(id);
            return View(model);
        }


        [PageInfo("Saha Görev Formu Düzenle", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var formResult = db.GetVWFTM_TaskFormResultById(id);
            return View(formResult);
        }

        [AllowEveryone]
        [PageInfo("Saha Görev Formu Düzenleme Metodu", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(Guid id, string jsonResult)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var rs = new ResultStatus { result = true };

            if (!String.IsNullOrEmpty(jsonResult))
            {
                var formresult = db.GetFTM_TaskFormResultById(id);
                if (formresult != null)
                {
                    formresult.jsonResult = jsonResult;
                    rs &= db.UpdateFTM_TaskFormResult(formresult);
                }
                else
                {
                    rs.result = false;
                    rs.message = "Form bulunamadı.";
                }
            }
            else
            {
                rs.result = false;
                rs.message = "Form sonuçları bulunamadı.";
            }
            var result = new ResultStatusUI
            {
                Result = rs.result,
                FeedBack = rs.result ? feedback.Success("Güncelleme işlemi başarılı.", false) : feedback.Error("Güncelleme işlemi başarısız." + rs.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Saha Görev Formu Detayı (Mobil)", SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var formResult = db.GetVWFTM_TaskFormResultById(id);
            return View(formResult);
        }



        [PageInfo("Saha Görev Form Raporu", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ContentResult GetResult(Guid formId, DateTime? startDate, DateTime? endDate, string inventories, string users, Guid? customerId, Guid? customerStorageId)
        {
            var db = new WorkOfTimeDatabase();
            var result = db.GetVWFTM_TaskFormResultByFormId(formId);

            if (startDate.HasValue && endDate.HasValue)
            {
                result = result.Where(a => a.created >= startDate && a.created <= endDate).ToArray();
            }

            if (!String.IsNullOrEmpty(users))
            {
                result = result.Where(a => users.Split(',').Select(s => Guid.Parse(s)).ToList().Contains(a.createdby.Value)).ToArray();
            }

            if (!String.IsNullOrEmpty(inventories))
            {
                result = result.Where(a => a.fixtureId.HasValue && inventories.Split(',').Select(s => Guid.Parse(s)).ToList().Contains(a.fixtureId.Value)).ToArray();
            }

            if (customerId.HasValue)
            {
                result = result.Where(x => x.customerId == customerId.Value).ToArray();
            }

            if (customerStorageId.HasValue)
            {
                result = result.Where(x => x.customerStorageId == customerStorageId.Value).ToArray();
            }

            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }
    }
}
