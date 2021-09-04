using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.DOC.Controllers
{
    public class VWDOC_DocumentRelationController : Controller
    {
      
        [PageInfo("Doküman İlişki Yönetimi Veri Methodu", SHRoles.DokumanYonetimRolu)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWDOC_DocumentRelation(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWDOC_DocumentRelationCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Doküman İlişki Ekleme", SHRoles.DokumanYonetimRolu)]
        public ActionResult Insert(VWDOC_DocumentRelation item)
        {
            return View(item);
        }

        [PageInfo("Doküman Ekleme Methodu", SHRoles.DokumanYonetimRolu)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(DOC_DocumentRelation item)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            if(item.documentId == item.documentRelationId)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Doküman kendisi ile ilişkili olamaz.")
                },JsonRequestBehavior.AllowGet);
            }

            var documentRelations = db.GetDOC_DocumentRelationByDocumentId(item.documentId.Value);
            if(documentRelations.Where(x => x.documentRelationId == item.documentRelationId).Count() > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Daha önce aynı doküman ile ilişki kurulmuş.")
                },JsonRequestBehavior.AllowGet);
            }

            var userStatus = (PageSecurity)Session["userStatus"];
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbres = db.InsertDOC_DocumentRelation(item);
            return Json(new ResultStatusUI
            {
                FeedBack = dbres.result ? feedback.Success("Doküman İlişki kurma işlemi başarılı") : feedback.Warning("Doküman ilişki kurma işlemi başarısız oldu."),
                Result = dbres.result
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
