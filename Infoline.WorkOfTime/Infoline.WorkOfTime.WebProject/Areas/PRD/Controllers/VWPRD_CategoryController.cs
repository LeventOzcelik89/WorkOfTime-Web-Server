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
    public class VWPRD_CategoryController : Controller
    {
        [PageInfo("Ürün Kategorileri", SHRoles.StokYoneticisi)]
        public ActionResult Index(Guid? id)
        {
            var db = new WorkOfTimeDatabase();
            var model = id.HasValue ? db.GetVWPRD_CategoryById(id.Value) : new VWPRD_Category
            {
                id = System.UIHelper.Guid.Null,
                name = "Ana Kategoriler",
                fullname = "Ana Kategoriler"
            };

            return View(model);
        }

        [PageInfo("Ürün Kategorileri Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_Category(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_CategoryCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün Kategorileri Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_Category(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ürün Kategorisi Ekleme", SHRoles.Personel)]
        public ActionResult Insert(VWPRD_Category item)
        {
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ürün Kategorisi Ekleme", SHRoles.Personel)]
        public JsonResult Insert(PRD_Category item, string Names)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var dbresult = new ResultStatus { result = true };

            if (String.IsNullOrEmpty(Names) && String.IsNullOrEmpty(item.name))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kategori kaydetme işlemi başarısız. Lütfen en az bir kategori ismi giriniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            if (!String.IsNullOrEmpty(Names))
            {
                var model = Names.Split(',').Select(a => new PRD_Category
                {
                    created = DateTime.Now,
                    id = Guid.NewGuid(),
                    createdby = userStatus.user.id,
                    name = a.ToString(),
                    pid = item.pid
                }).ToArray();

                dbresult = db.BulkInsertPRD_Category(model);
            }

            if (!String.IsNullOrEmpty(item.name))
            {
                item.created = DateTime.Now;
                item.createdby = userStatus.user.id;

                dbresult = db.InsertPRD_Category(item);
            }

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Ürün Kategorisi Güncelleme", SHRoles.StokYoneticisi)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_CategoryById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ürün Kategorisi Güncelleme", SHRoles.StokYoneticisi)]
        public JsonResult Update(PRD_Category item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdatePRD_Category(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Ürün Kategorisi Silme", SHRoles.StokYoneticisi)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var item = db.GetPRD_CategoryByPid(id);
            if (item.Count() > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kategorinin alt kategorileri bulunduğu için kaydı silemezsiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var category = db.GetPRD_CategoryById(id);
                var dbresult = db.DeletePRD_Category(category);

                return Json(new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarısız")
                }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}
