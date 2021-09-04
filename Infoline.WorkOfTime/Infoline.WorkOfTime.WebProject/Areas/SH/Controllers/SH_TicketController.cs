using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;


namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class SH_TicketController : Controller
    {
        [PageInfo("Kullanıcı Oturum Raporu", SHRoles.SistemYonetici, SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
        public ActionResult Dashboard()
        {
            return View();
        }

        [PageInfo("Kullanıcı Giriş Çıkışları Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            if(request.Sorts.Count() == 0)
            {
                var kendo = new SortDescriptor();
                kendo.Member = "createtime";
                kendo.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                request.Sorts.Add(kendo);
            }

            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_Ticket(condition).ToDataSourceResult(request);
            data.Total = db.GetVWSH_TicketCount(condition.Filter);

            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [PageInfo("Çevrimiçi Kullanıcılar Sayfası Grafikler", SHRoles.SistemYonetici, SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
        public ContentResult DashboardResult()
        {

            var db = new WorkOfTimeDatabase();
            var now = DateTime.Now;
            var personsTotalSession = db.GetVWSH_Ticket().Where(x => x.userid != Guid.Empty);
            var timeline = db.SH_TicketByMonthsTimeAlternate();
            var onlinePersonNow = personsTotalSession.Where(a => a.createtime <= now && a.endtime >= now).OrderBy(a => a.User_Title);
            var personSessions = personsTotalSession.GroupBy(x => x.User_Title).Select(a => new
            {
                Key = a.Key,
                Value = a.Count()
            }).OrderByDescending(x => x.Value).Take(5).ToArray();
            var personToday = personsTotalSession.Where(a => a.createtime.Value.Date == DateTime.Now.Date).GroupBy(x => x.User_Title).Select(s => new
            {
                Key = s.Key,
                Value = s.Count()
            }).OrderByDescending(x => x.Value).ToArray();
            foreach (var item in timeline)
            {
                var s = item.DateMonthYear.Split('-');
                item.DateMonthYear = s[0] + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(s[1]));
            }
            var data = new
            {
                personNowOnline = onlinePersonNow.ToArray(),
                personSessions = personSessions.ToArray(),
                personToday = personToday.ToArray(),
                personOnline = timeline.ToArray()
            };
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
    }
}

