using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_BrandController : Controller
    {
        [PageInfo("Ürün Marka Tanımları", SHRoles.StokYoneticisi)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            return View();
        }

        [PageInfo("Ürün Marka Tanımları Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_Brand(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_BrandCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Ürün Marka Tanımları Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_Brand(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün Marka Tanımları Ekleme", SHRoles.Personel)]
        public ActionResult Insert(VWPRD_Brand model)
        {
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ürün Marka Tanımları Ekleme", SHRoles.Personel)]
        public JsonResult Insert(PRD_Brand item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var nameControl = db.GetPRD_BrandByName(item.name);
            if (nameControl != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı Marka Kaydı Zaten Sistemde Bulunmaktadır.")
                }, JsonRequestBehavior.AllowGet);
            }

            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var dbresult = db.InsertPRD_Brand(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Ürün Marka Tanımları Güncelleme", SHRoles.StokYoneticisi)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_BrandById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ürün Marka Tanımları Güncelleme", SHRoles.StokYoneticisi)]
        public JsonResult Update(PRD_Brand item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new ResultStatus();

            if (db.GetPRD_BrandByNameByUpdateName(item.id, item.name) == null)
            {
                item.changed = DateTime.Now;
                item.changedby = userStatus.user.id;

                dbresult = db.UpdatePRD_Brand(item);
            }
            else
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Aynı kayıt zaten mevcut. Lütfen farklı bir marka adı giriniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Ürün Marka Tanımları Silme", SHRoles.StokYoneticisi)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new PRD_Brand { id = new Guid(a) });

            var productsByBrand = db.GetPRD_ProductsByBrandIds(item.Select(a => a.id).ToArray());

            if (productsByBrand.Count() > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Silmeye çalıştığınız markaların ürünleri bulunmaktadır. Lütfen önce ürünleri siliniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            var dbresult = db.BulkDeletePRD_Brand(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Excelden Marka Ekleme")]
        public JsonResult Import(string model)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var culture = new System.Globalization.CultureInfo("tr-TR", false);

            var dataModel = Infoline.Helper.Json.Deserialize<PRD_BrandExcel[]>(model);

            var brandNames = db.GetPRD_Brand().Select(a => a.name.ToLower(culture)).ToArray();

            var data = dataModel.Where(a => !brandNames.Contains(a.name.Trim().ToLower(culture))).Select(a => new PRD_Brand
            {
                id = Guid.NewGuid(),
                name = a.name,
                created = DateTime.Now,
                createdby = userStatus.user.id,
            }).ToArray();

            var rs = db.BulkInsertPRD_Brand(data);

            var result = new ResultStatusUI
            {
                Result = rs.result,
                FeedBack = rs.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
