using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
    public class VWFTM_TaskGridController : Controller
    {
        [PageInfo("Detaylı Rapor", SHRoles.SahaGorevMusteri,SHRoles.SahaGorevYonetici,SHRoles.SahaGorevOperator)]
        public ActionResult Index(string taskIds)
        {
            var ids = new List<Guid>();
            if (!String.IsNullOrEmpty(taskIds) && !taskIds.Contains("undefined"))
            {
                var taskIdsData = taskIds.Split(',');
                foreach (var taskid in taskIdsData)
                {
                    ids.Add(new Guid(taskid));
                }
            }
            return View(ids);
        }
        [PageInfo("Tüm Saha Görevleri Veri Methodu", SHRoles.Personel, SHRoles.SahaGorevMusteri)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus, 1);
            var data = db.GetVWFTM_TaskGrid(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWFTM_TaskGridCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Scada Test Kullanıcısı için view", SHRoles.Personel)]
        public ActionResult ScadaTasks()
        {
            return View();
        }
    }
}
