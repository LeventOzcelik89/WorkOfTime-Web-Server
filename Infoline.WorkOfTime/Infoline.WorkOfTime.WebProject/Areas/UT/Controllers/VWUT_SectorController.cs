using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.UT.Controllers
{
    public class VWUT_SectorController : Controller
    {
        [PageInfo("Sektör Tanımları", SHRoles.SistemYonetici)]
        public ActionResult Index(Guid? id)
        {

            var db = new WorkOfTimeDatabase();
            var model = id.HasValue ? db.GetVWUT_SectorById(id.Value) : new VWUT_Sector
            {
                id = System.UIHelper.Guid.Null,
                name = "Ana Sektörler",
                fullname = "Ana Sektörler"
            };
            ViewBag.Card = db.GetUT_SectorCard();
            return View(model);
        }

        [PageInfo("Sektör Tanımları Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWUT_Sector(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWUT_SectorCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Sektör Tanımları Veri Methodu", SHRoles.Personel,SHRoles.CRMBayiPersoneli,SHRoles.CagriMerkezi,SHRoles.HakEdisBayiPersoneli)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWUT_Sector(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Sektör Tanımları Ekleme", SHRoles.Personel)]
        public ActionResult Insert(VWUT_Sector item)
        {
            return View(item);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Sektör Tanımları Ekleme", SHRoles.Personel)]
        public JsonResult Insert(UT_Sector item, string Names)
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
                    FeedBack = feedback.Warning("Sektör kaydetme işlemi başarısız. Lütfen en az bir sektör ismi giriniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            if (!String.IsNullOrEmpty(Names))
            {
                var model = Names.Split(',').Select(a => new UT_Sector
                {
                    created = DateTime.Now,
                    id = Guid.NewGuid(),
                    createdby = userStatus.user.id,
                    name = a.ToString(),
                    pid = item.pid
                }).ToArray();

                dbresult = db.BulkInsertUT_Sector(model);
            }

            if (!String.IsNullOrEmpty(item.name))
            {
                item.created = DateTime.Now;
                item.createdby = userStatus.user.id;

                dbresult = db.InsertUT_Sector(item);
            }

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Sektör Tanımları Güncelleme", SHRoles.SistemYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWUT_SectorById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Sektör Tanımları Güncelleme", SHRoles.SistemYonetici)]
        public JsonResult Update(UT_Sector item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateUT_Sector(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Sektör Tanımları Silme", SHRoles.SistemYonetici)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var item = db.GetUT_SectorByPid(id);
            if (item.Count() > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Sektörün alt sektörleri bulunduğu için kaydı silemezsiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var sector = db.GetUT_SectorById(id);
                var dbresult = db.DeleteUT_Sector(sector);

                return Json(new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarısız")
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
