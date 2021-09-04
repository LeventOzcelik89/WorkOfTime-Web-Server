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
    public class VWDOC_DocumentRevisionRequestController : Controller
    {

        [PageInfo("Tüm Revizyon Talepleri", SHRoles.SistemYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Revizyon Taleplerim", SHRoles.DokumanYonetimRolu)]
        public ActionResult MyIndex()
        {
            return View();
        }

        [PageInfo("Revizyon Talepleri (Onay)", SHRoles.DokumanYonetimRolu)]
        public ActionResult MyAboutIndex()
        {
            return View();
        }

        [PageInfo("Revizyon Talepleri (Onay) Adet Methodu", SHRoles.DokumanYonetimRolu)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWDOC_DocumentRevisionRequestCount(condition.Filter);
            return count;
        }

        [PageInfo("Doküman Revizyon Talep Veri Methodu", SHRoles.DokumanYonetimRolu)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWDOC_DocumentRevisionRequest(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWDOC_DocumentRevisionRequestCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Doküman Revizyon Silme işlemi", SHRoles.DokumanYonetimRolu)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedBack = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            var req = db.GetDOC_DocumentRevisionRequestById(id);
            if (req == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedBack.Warning("Revizyon talebi bulunamadı.")
                }, JsonRequestBehavior.AllowGet);
            }

            if (req.createdby != userStatus.user.id)
            {
                if (userStatus.user.id != Guid.Empty)
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedBack.Warning("Revizyon talebini sadece talebi gerçekleştirenler silebilir.")
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            var reqConfirmation = db.GetDOC_DocumentRevisionRequestConfirmationByRequestId(id);
            if (reqConfirmation.Count(x => x.status.HasValue) > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedBack.Warning("Revizyon talebinin onay süreci başladığından silme işlemini gerçekleştiremezsiniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            var res = db.DeleteDOC_DocumentRevisionRequest(req);
            return Json(new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result ? feedBack.Success("Revizyon talebi silme işlemi başarılı") : feedBack.Warning("Revizyon talebi silme işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
