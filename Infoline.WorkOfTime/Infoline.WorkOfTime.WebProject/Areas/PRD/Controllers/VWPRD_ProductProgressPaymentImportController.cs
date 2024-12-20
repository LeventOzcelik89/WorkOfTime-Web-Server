﻿using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_ProductProgressPaymentImportController : Controller
    {
        [PageInfo("Bayi Satış Listesi", SHRoles.SistemYonetici,SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli)]
        public ActionResult Index(string ids)
        {
            var _ids = new List<Guid>();
            if (!String.IsNullOrEmpty(ids))
            {
                _ids = ids.Split(',').Select(a => new Guid(a)).ToList();
            }
            return View(_ids);
        }

        [PageInfo("Veri Kaynağı", SHRoles.SistemYonetici, SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.MusteriSatisSorumlusu)))
            {
                var cc = KendoToExpression.Convert(request);
                cc = VMPRD_ProductProgressPaymentImportModel.UpdateQueryManaging(cc, userStatus);
                var datam = db.GetVWPRD_ProductProgressPaymentImport(cc).RemoveGeographies().ToDataSourceResult(request);
                datam.Total = db.GetVWPRD_ProductProgressPaymentImportCount(cc.Filter);
                return Content(Infoline.Helper.Json.Serialize(datam), "application/json");
            }
            var data = db.GetVWPRD_ProductProgressPaymentImport(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_ProductProgressPaymentImportCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Veri Kaynağı Dropdown", SHRoles.SistemYonetici, SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_ProductProgressPaymentImport(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Bayi Satış Oluştur", SHRoles.SistemYonetici, SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli)]
        public ActionResult Insert(VMPRD_ProductProgressPaymentImportModel model)
        {
            var data = model.Load();
            return View(data);
        }

        [PageInfo("Bayi Satış Oluştur", SHRoles.SistemYonetici, SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMPRD_ProductProgressPaymentImportModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Hakediş Tanımlama İşlemi Başarılı") : feedback.Warning(dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Excel Bayi Satış Oluştur", SHRoles.SistemYonetici, SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli)]
        [HttpPost]
        public JsonResult Import(string model)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = VMPRD_ProductProgressPaymentImportModel.Import(model, userStatus.user.id);
            var result =  new ResultStatusUI
            {
                Result = dbresult.result,
                Object = dbresult.objects,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Bayi Satış Sil", SHRoles.SistemYonetici, SHRoles.HakEdisBayiPersoneli, SHRoles.PrimHakedisPersoneli)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            return Json(new ResultStatusUI(new VMPRD_ProductProgressPaymentImportModel { id = id }.Delete()), JsonRequestBehavior.AllowGet);
        }
    }
}
