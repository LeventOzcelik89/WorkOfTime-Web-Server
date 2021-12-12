using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Tools.Mapper.Maps;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_UserReportController : Controller
    {
        [PageInfo("Detaylı Kullanıcı Raporu Listeleme Sayfası", SHRoles.IKYonetici)]
        public ActionResult Index(Guid? id)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            if (id.HasValue)
            {
                var db = new WorkOfTimeDatabase();
                var model = new PageModel<VWSH_User,VWSH_Report> { Name = "Report",Title= "Detaylı Kullanıcı Raporu" };
                var report = db.GetVWSH_ReportById(id.Value);
                if (report != null && report.title != null)
                {
                    model.Title = report.title;
                }
                model.PropertyBag = report;
                return View(model);
            }
            else
            {
                var model = new PageModel<VWSH_User, VWSH_Report> { Name = "Report", Title = "Detaylı Kullanıcı Raporu" };

                model.PropertyBag = new VWSH_Report();
                return View(model);
            }
        }
        [PageInfo("Detaylı Kullanıcı Raporu Listeleme Metodu", SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_UserReport(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_UserReportCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Detaylı Kullanıcı Raporu Tablo Alanları", SHRoles.IKYonetici)]
        public ContentResult GetFilter()
        {
            var items = new VWSH_UserReportMapper().GetFields();
            return Content(Infoline.Helper.Json.Serialize(items), "application/json");
        }
    }
}
