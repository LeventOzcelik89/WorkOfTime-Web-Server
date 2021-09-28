using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_ProductionController : Controller
    {
        [PageInfo("Üretim Emirleri", SHRoles.UretimYonetici, SHRoles.UretimPersonel)]
        public ActionResult Index()
        {
            return View();
        }


        [PageInfo("Üretim Emri Oluştur", SHRoles.UretimYonetici)]
        public ActionResult Insert(VMPRD_ProductionModel data)
        {
            return View(data.Load());
        }

        [HttpPost]
        [PageInfo("Üretim Emri Oluştur", SHRoles.UretimYonetici)]
        public JsonResult Insert(VMPRD_ProductionModel data, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new ResultStatus { result = true };

            dbresult = data.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Üretim emri başarıyla oluşturuldu.") :
                                             feedback.Warning(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Üretim emri oluşturma işlemi başarısız oldu."),
                Object = data.id
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Üretim Emri Güncelle", SHRoles.UretimYonetici)]
        public ActionResult Update(VMPRD_ProductionModel data)
        {
            return View(data.Load());
        }

        [HttpPost]
        [PageInfo("Üretim Emri Güncelle", SHRoles.UretimYonetici)]
        public JsonResult Update(VMPRD_ProductionModel data, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new ResultStatus { result = true };

            dbresult = data.Save(userStatus.user.id, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Üretim emri başarıyla oluşturuldu.") :
                                             feedback.Warning(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Üretim emri oluşturma işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Üretim Emri Detay", SHRoles.UretimYonetici, SHRoles.UretimPersonel)]
        public ActionResult Detail(VMPRD_ProductionModel request)
        {
            var data = request.Load();
            return View(data);
        }


        [PageInfo("Tüm Üretim Emirleri", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var userStatus = (PageSecurity)Session["userStatus"];
            condition = VMPRD_ProductionModel.UpdateQuery(condition, userStatus);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_Production(condition).ToDataSourceResult(request);
            data.Total = db.GetVWPRD_ProductionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Üretim Emri Sil)", SHRoles.UretimYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var res = new VMPRD_ProductionModel { id = id }.Delete(userStatus.user.id);

            var result = new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result == false ? feedback.Warning(res.message) : feedback.Success("Üretim emri silme işlemi başarılı")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Stok&Envanter İşlem Girişi", SHRoles.Personel, SHRoles.UretimYonetici)]
        public ActionResult Upsert(VMPRD_ProductionModel model, VWPRD_Transaction trans, int? direction)
        {
            model.Transaction = trans;
            var data = model.Load();
            ViewBag.Direction = direction;
            return View(data);
        }

        [PageInfo("Stok&Envanter İşlemi Ekleme ve Güncelleme", SHRoles.UretimYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Upsert(VMPRD_ProductionModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new ResultStatus { result = true };
            if (item.Transaction.type == (int)EnumPRD_TransactionType.FireFisi)
            {
                dbresult = item.InsertForFireNotification(userStatus.user.id);
            }
            if (item.Transaction.type == (int)EnumPRD_TransactionType.HarcamaBildirimi)
            {
                dbresult = item.InsertForSpendProducts(userStatus.user.id);
            }


            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message,false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Stok&Envanter İşlem Girişi", SHRoles.UretimYonetici, SHRoles.UretimPersonel)]
        public ActionResult FinishedProductNotification(VMPRD_ProductionModel model, VWPRD_Transaction trans, int? direction)
        {
            model.Transaction = trans;
            model.Transaction.status = (int)EnumPRD_TransactionStatus.beklemede;
            model.Transaction.type = (int)EnumPRD_TransactionType.GelenIrsaliye;
            var data = model.Load();
            ViewBag.Direction = direction;
            return View(data);
        }

        [PageInfo("Stok&Envanter İşlemi Ekleme ve Güncelleme", SHRoles.UretimYonetici, SHRoles.UretimPersonel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult FinishedProductNotification(VMPRD_ProductionModel item, bool? isPost)
        {

            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.Transaction.type = (int)EnumPRD_TransactionType.UretimBildirimi;
            var dbresult = item.InsertForFinishedProducts(userStatus.user.id);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message,false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Ürünlere ait depolarda ki stok miktarlarıni dönen method", SHRoles.Personel)]
        public JsonResult GetProductStocksByProductIdsAndStorageId(Guid[] productIds, Guid storageId)
        {
            if (productIds!=null)
            {
                var model = new VMPRD_ProductionModel().ProductStocksByProductIdsAndStorageId(productIds, storageId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<VWPRD_StockSummary>(), JsonRequestBehavior.AllowGet);

        }


        [PageInfo("Üretime ait ürünler ve stok hareketlerinde ki harcamaları dönen method", SHRoles.Personel)]
        public JsonResult ProductionProductAndTransactionDataSource(Guid productionId)
        {
            var productionProducts = new VMPRD_ProductionModel().GetProductionProductAndTransaction(productionId);
            return Json(productionProducts, JsonRequestBehavior.AllowGet);
        }
    }
}