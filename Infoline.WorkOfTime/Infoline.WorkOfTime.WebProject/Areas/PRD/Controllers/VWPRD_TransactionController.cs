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
    public class VWPRD_TransactionController : Controller
    {
        [PageInfo("Stok ve Envanter İşlemleri", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
        public ActionResult Index()
        {
            return View();
        }
        [PageInfo("Stok&Envanter İşlemleri Grid DataSource", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli, SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_Transaction(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_TransactionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Stok&Envanter İşlemleri Miktar DataSource", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            return db.GetVWPRD_TransactionCount(condition.Filter);
        }

        [PageInfo("Stok&Envanter İşlemleri Dropdown DataSource", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_Transaction(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Envanterler DataSource", SHRoles.Personel)]
        public ContentResult DataSourceInventoryCodes(Guid? lastActionDataId, Guid? lastActionDataCompanyId, Guid productId)
        {
            var db = new WorkOfTimeDatabase();
            var res = db.GetVWPRD_InventoryByProductIdLastActionId(productId, lastActionDataCompanyId, lastActionDataId).OrderBy(a => a);
            return Content(Infoline.Helper.Json.Serialize(res), "application/json");
        }


        [PageInfo("Envanter Detayı", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.IKYonetici, SHRoles.UretimPersonel, SHRoles.UretimYonetici, SHRoles.Personel)]
        public ActionResult Detail(VMPRD_TransactionModel model)
        {
            var data = model.Load();
            return View(data);
        }

        [PageInfo("Form Yükleme Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.IKYonetici)]
        public ActionResult UploadForm(VMPRD_TransactionModel model)
        {
            var data = model.Load();
            return View(data);
        }

        [PageInfo("Form Yükleme Methodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.IKYonetici)]
        [HttpPost]
        public JsonResult UploadFormPost(VMPRD_TransactionModel item, bool? ispost)
        {
            var feedback = new FeedBack();
            var res = new FileUploadSave(Request).SaveAs();
            return Json(new ResultStatusUI
            {
                Result = res.Result,
                FeedBack = res.Result ? feedback.Success("Dosya yükleme işlemi başarılı") : feedback.Warning("Dosya yükleme işlemi başarısız oldu")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Stok&Envanter İşlem Girişi", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.IKYonetici)]
        public ActionResult Upsert(VMPRD_TransactionModel model, int? direction)
        {
            var data = model.Load();

            //Sadece teklifden gelirken kullanılıyor.
            if (!string.IsNullOrEmpty(model.tenderIds))
            {
                data.items = new List<VMPRD_TransactionItems>();
                var db = new WorkOfTimeDatabase();
                var tenders = db.GetVWCMP_TenderByIds(model.tenderIds.Split(',').Select(x => Guid.Parse(x)).ToArray());
                foreach (var tender in tenders)
                {
                    var tenderItems = db.GetCMP_InvoiceItemByInvoiceId(tender.id);
                    data.items.AddRange(tenderItems.Select(x => new VMPRD_TransactionItems
                    {
                        unitPrice = x.price,
                        quantity = x.quantity,
                        productId = x.productId
                    }).ToList());
                }
            }

            if (data.items.Count() == 1 && !data.items.Select(x => x.productId.HasValue).FirstOrDefault())
            {
                data.items = new List<VMPRD_TransactionItems>();
            }
            ViewBag.Direction = direction;
            return View(data);
        }

        [PageInfo("Stok&Envanter İşlemi Ekleme ve Güncelleme", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SatisPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Upsert(VMPRD_TransactionModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Envanter Zimmet Alma", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu)]
        public ActionResult InsertEmbezzlePerson(Guid personId)
        {
            var db = new WorkOfTimeDatabase();
            var inventories = db.GetVWPRD_InventoryByLastActionDataId(personId);
            var model = new VMPRD_TransactionModel().Load();
            model.items = inventories.GroupBy(x => x.productId).Select(x => new VMPRD_TransactionItems
            {
                productId = x.Key,
                productId_Title = x.Select(c => c.productId_Title).FirstOrDefault(),
                serialCodes = string.Join(",", x.Select(c => c.serialcode).ToArray()),
                quantity = x.Count()
            }).ToList();
            model.type = (int)EnumPRD_TransactionType.ZimmetAlma;
            model.type_Title = "Zimmet Alma";
            model.outputId = personId;
            model.outputTable = "SH_User";
            return PartialView("Upsert", model);
        }

        [PageInfo("Envanter Zimmet İşlemi", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu)]
        public ActionResult InsertEmbezzle(Guid inventoryId)
        {
            var db = new WorkOfTimeDatabase();
            var inventory = db.GetVWPRD_InventoryById(inventoryId);
            var model = new VMPRD_TransactionModel().Load();
            model.date = DateTime.Now;
            model.items = new List<VMPRD_TransactionItems> {
                new VMPRD_TransactionItems{
                    productId = inventory.productId,
                    productId_Title = inventory.productId_Title,
                    serialCodes = inventory.serialcode,
                    quantity = 1,
                    unitPrice = 0,
                },
            };

            if (inventory.lastActionType == (int)EnumPRD_InventoryActionType.Depoda)
            {
                model.type = (int)EnumPRD_TransactionType.ZimmetVerme;
                model.type_Title = "Zimmet Verme";
                model.outputId = inventory.lastActionDataId;
                model.outputTable = "CMP_Storage";
            }
            else
            {
                model.type = (int)EnumPRD_TransactionType.ZimmetAlma;
                model.type_Title = "Zimmet Alma";
                model.outputId = inventory.lastActionDataId;
                model.outputTable = "SH_User";
            }
            return PartialView("Upsert", model);
        }

        [PageInfo("Stok&Envanter İşlem Yazdır", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi), ExportPDF]
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
        [PageInfo("Stok&Envanter İşlem Yazdır", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
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

        [PageInfo("Ürününe ait daha önce seri kodu kullanıldığını kontrol eden metod", SHRoles.Personel)]
        public JsonResult IsSerialUserBefore(string serial, Guid productId)
        {
            {
                var result = new VMPRD_TransactionModel().IsSerialUsedBefore(serial, productId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

