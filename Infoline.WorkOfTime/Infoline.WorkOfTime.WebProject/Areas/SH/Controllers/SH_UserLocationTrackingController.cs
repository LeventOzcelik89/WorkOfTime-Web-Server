using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class SH_UserLocationTrackingController : Controller
    {
        [PageInfo("Personel Takip Haritası", SHRoles.IdariPersonelYonetici)]
        public ActionResult Map()
        {
            return View();
        }

        [PageInfo("Personel İzleme Haritası Data Metodu", SHRoles.IdariPersonelYonetici)]
        public ContentResult GetMapData(DateTime startDate, DateTime endDate, Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var trackingDatas = new SH_UserLocationTrackingMap();
            var locationTrackingDatas = db.GetVWUT_LocationTrackingByUserIdAndDates(userId, startDate, endDate);
            if (locationTrackingDatas.Count() > 0)
            {
                var user = db.GetVWSH_UserById(userId);
                trackingDatas.LocationTrackings = locationTrackingDatas;
                trackingDatas.FullName = user.FullName;
                trackingDatas.id = user.id;
                trackingDatas.Title = user.title;
                trackingDatas.ProfilePhoto = user.ProfilePhoto;
                trackingDatas.Department_Title = user.Department_Title;
            }
            return Content(Infoline.Helper.Json.Serialize(trackingDatas), "application/json");
        }
    }
}