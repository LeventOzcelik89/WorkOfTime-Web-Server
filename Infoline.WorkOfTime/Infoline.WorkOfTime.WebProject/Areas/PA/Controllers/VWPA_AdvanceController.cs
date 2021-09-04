using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PA.Controllers
{
    public class VWPA_AdvanceController : Controller
    {
        [PageInfo("Avans Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_Advance(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPA_AdvanceCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Avans Sayıları Methodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var total = db.GetVWPA_AdvanceCount(condition.Filter);
            return total;
        }

        [PageInfo("Avans Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_Advance(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Avans Detay Sayfası", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
        {
            var model = new VMPA_AdvanceModel { id = id }.Load();
            model.direction = (int)EnumPA_TransactionDirection.Cikis;
            return View(model);
        }

        [PageInfo("Avans Talepleri (Onay)", SHRoles.Personel)]
        public ActionResult IndexRequest()
        {
            return View();
        }

        [PageInfo("Avans Taleplerim", SHRoles.MuhasebeAlis, SHRoles.MuhasebeSatis, SHRoles.OnMuhasebe)]
        public ActionResult IndexAllRequest()
        {
            return View();
        }

        [PageInfo("Avans Taleplerim Sayfası", SHRoles.Personel)]
        public ActionResult IndexMyRequest()
        {
            return View();
        }

        [PageInfo("Avans Ekleme Sayfası", SHRoles.Personel)]
        public ActionResult InsertExpense(VMPA_AdvanceModel item)
        {
            item.Load();
            item.direction = (short)EnumPA_AdvanceDirection.Talep;

            return View(item);
        }

        [PageInfo("Avans Düzenleme Sayfası", SHRoles.Personel)]
        public ActionResult UpdateExpense(Guid id, bool? IsCopy = false)
        {
            var db = new WorkOfTimeDatabase();
            var model = new VMPA_AdvanceModel { id = id, IsCopy = IsCopy }.Load();
            model.Ledgers = db.GetPA_LedgerByAdvanceId(id);
            model.status = (int)EnumPA_AdvanceStatus.Odenecek;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ödeme Planı Ekleme Sayfası", SHRoles.Personel)]
        public JsonResult Insert(VMPA_AdvanceModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Avans Güncelleme Sayfası", SHRoles.Personel)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPA_AdvanceById(id);
            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Avans Güncelleme Sayfası", SHRoles.Personel)]
        public JsonResult Update(VMPA_AdvanceModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Avans Toplu Onaylama Methodu", SHRoles.Personel)]
        public JsonResult AllApprovalAdvance([DataSourceRequest] DataSourceRequest request, string ids)
        {
            var db = new WorkOfTimeDatabase();
            var feedBack = new FeedBack();
            var condition = KendoToExpression.Convert(request);
            condition.Take = 99999;
            condition.Skip = 0;
            var advances = new List<VWPA_Advance>();
            var userStatus = (PageSecurity)Session["userStatus"];
            if (string.IsNullOrEmpty(ids))
            {
                advances = db.GetVWPA_Advance(condition).ToList();
            }
            else
            {
                var advanceIds = ids.Split(',').Select(a => Guid.Parse(a)).ToArray();
                advances = db.GetVWPA_AdvanceByIds(advanceIds).ToList();
            }


            advances = advances.Where(x => (x.confirmationStatus == (Int16)EnumPA_AdvanceConfirmationStatus.Onay || x.confirmationStatus == null) || (x.confirmUserIds.Contains(userStatus.user.id.ToString()) && x.direction == (Int16)EnumPA_AdvanceDirection.Talep)).ToList();


            if (advances.Count() == 0)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = feedBack.Warning("Onaylanacak avans talebi bulunamadı.") }, JsonRequestBehavior.AllowGet);
            }

            var res = new ResultStatus { result = true };

            foreach (var advance in advances)
            {
                advance.direction = -1;
                advance.dataId = null;
                res &= new VMPA_AdvanceModel().B_EntityDataCopyForMaterial(advance).Save(userStatus.user.id);
            }

            return Json(new ResultStatusUI { Result = res.result, FeedBack = res.result ? feedBack.Success("Avans talepleri başarı ile onaylandı") : feedBack.Warning("Avans onaylama işlemi başarısız oldu.") }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Ödeme Planı Silme İşlemi", SHRoles.OnMuhasebe)]
        public JsonResult Delete(Guid id)
        {
            var feedback = new FeedBack();
            var rs = new VMPA_AdvanceModel { id = id }.Load().Delete();

            return Json(new ResultStatusUI
            {
                Result = rs.result,
                FeedBack = rs.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}


