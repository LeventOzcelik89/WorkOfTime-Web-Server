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

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_ProductPaymentController : Controller
	{
		[PageInfo("Hakediş Raporu", SHRoles.SistemYonetici)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Hakediş Raporu Veri Kaynağı", SHRoles.SistemYonetici)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductPayment(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWPRD_ProductPaymentCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Hakediş Raporu Detay", SHRoles.SistemYonetici)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWPRD_ProductPaymentById(id);
		    return View(data);
		}
        //[PageInfo("Hakediş Raporu Veri kaynağı", SHRoles.SistemYonetici)]
        //public JsonResult ReportDataSource(Guid companyId, int year, int month)
        //{
        //    var model = new VMPRD_ProductPaymentModel();
        //    var data = model.Data
        //    var db = new WorkOfTimeDatabase();
        //    var getCompany = db.GetCMP_CompanyById(companyId);
        //    if (getCompany == null)
        //    {
        //        return Json(new ResultStatusUI
        //        {
        //            Result = false,
        //            Object = "0",
        //            FeedBack = new FeedBack().Warning("Böyle bir bayi bulunmamaktadır.")
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    var bounty = new List<VWPRD_ProductBounty>();
        //    var getCompanyBounty = db.GetVWPRD_ProductBountyByPeriodAndCompanyId(month, year, companyId);
        //    if (getCompanyBounty.Length == 0)
        //    {
        //        if (getCompany.pid.HasValue)
        //        {
        //            var getDistBounty = db.GetVWPRD_ProductBountyByPeriodAndCompanyId(month, year, getCompany.pid.Value);
        //            if (getDistBounty.Length > 0)
        //            {
        //                bounty.AddRange(getDistBounty);
        //            }
        //            else
        //            {
        //                return Json(new ResultStatusUI
        //                {
        //                    Result = false,
        //                    Object = "1",
        //                    FeedBack = new FeedBack().Warning("Cariye veya distribütöre atanmış bu aya ait prim tanımı yoktur.")
        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            return Json(new ResultStatusUI
        //            {
        //                Result = false,
        //                Object = "1",
        //                FeedBack = new FeedBack().Warning("Cariye veya distribütöre atanmış bu aya ait prim tanımı yoktur.")
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        bounty.AddRange(getCompanyBounty);
        //    }
            
        //    //var returnObject = new
        //    //{
        //    //    counts,
        //    //    grid,
        //    //    total
        //    //};
        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}
    }
}
