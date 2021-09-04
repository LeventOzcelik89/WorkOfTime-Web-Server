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
    public class VWUT_LocationController : Controller
    {
        [PageInfo("Lokasyonlar (Ülke/İl/İlçe)",SHRoles.SistemYonetici)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var model = db.GetUT_LocationCard();
            return View(model);
        }

        [PageInfo("Ülke, Şehir, İlçe Methodu",SHRoles.Personel, SHRoles.AdayOgrenciYonetim)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWUT_Location(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWUT_LocationCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ülke, Şehir, İlçe Veri Methodu", SHRoles.Personel,SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent, SHRoles.BayiPersoneli)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWUT_Location(condition);

            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Ülke, Şehir, İlçe Detayı", SHRoles.SistemYonetici)]
        public ActionResult Detail(VWUT_Location data)
        {
            var db = new WorkOfTimeDatabase();
            var datas = db.GetVWUT_LocationById(data.id);
            datas.type = data.type;
            ViewBag.Card = db.GetUT_LocationCard();
            return View(datas);
        }


        [PageInfo("Ülke, Şehir, İlçe Ekleme", SHRoles.Personel)]
        public ActionResult Insert(VWUT_Location item)
        {
            return View(item);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Ülke, Şehir, İlçe Ekleme", SHRoles.Personel)]
        public JsonResult Insert(UT_Location item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertUT_Location(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Ülke, Şehir, İlçe Güncelleme", SHRoles.SistemYonetici)]
        public ActionResult Update(VWUT_Location item)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWUT_LocationById(item.id);
            data.type = item.type;
            return View(data);
        }


        [PageInfo("Ülke, Şehir, İlçe Güncelleme", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(UT_Location item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateUT_Location(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Ülke, Şehir, İlçe Silme", SHRoles.SistemYonetici)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var item = db.GetUT_LocationByPid(id);
            if (item.Count() > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Ülke, Şehir, İlçe vs. alt kaydı bulunduğu için kaydı silemezsiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var location = db.GetUT_LocationById(id);
                var dbresult = db.DeleteUT_Location(location);

                return Json(new ResultStatusUI
                {
                    Result = dbresult.result,
                    FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarısız.")
                }, JsonRequestBehavior.AllowGet);

            }

        }

    }
}
