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

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_AgileBoardsController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_AgileBoards(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_AgileBoardsCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_AgileBoards(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_AgileBoardsById(id);
            return View(data);
        }

        [PageInfo("Yeni Kanban Tanımlama", SHRoles.SistemYonetici, SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
        public ActionResult Insert(VWAgileBoardModel item)
        {
            item.Load();

            return View(item);
        }

        [PageInfo("Yeni Kanban Tanımlama", SHRoles.SistemYonetici, SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VWAgileBoardModel item, bool? isPost)
        {

            var userStatus = (PageSecurity)Session["userStatus"];
            var dbresult = item.Save(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? new FeedBack().Success(dbresult.message) : new FeedBack().Error(dbresult.message)
            };

            result.Object = item;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_AgileBoardsById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SH_AgileBoards item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateSH_AgileBoards(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Tanımlı Kanban Sil", SHRoles.SistemYonetici, SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new SH_AgileBoards { id = new Guid(a) });

            var dbresult = db.BulkDeleteSH_AgileBoards(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}
