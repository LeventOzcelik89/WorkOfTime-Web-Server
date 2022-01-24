using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class SH_UserLocationTrackingController : Controller
    {
        [PageInfo("Personel Takip Haritası", SHRoles.IdariPersonelYonetici)]
        public ActionResult Map(UTLocationUserFilter model)
        {
            return View(model);
        }

        [PageInfo("Personel Takip Haritası", SHRoles.IdariPersonelYonetici)]
        public ActionResult MapAll()
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

        [PageInfo("Personel İzleme Haritası Data Metodu", SHRoles.IdariPersonelYonetici)]
        public ContentResult GetMapDatas()
        {

            //  Get LocationConfigUsers


            var db = new WorkOfTimeDatabase();
            var locationTrackingDatas = db.GetVWUT_LocationConfigUser().ToList();

            return Content(Infoline.Helper.Json.Serialize(locationTrackingDatas), "application/json");
        }



        //[AllowEveryone]
        //public ContentResult RedMark(Guid id)//// bool ccc ekleyip arka tarafa green resmini getir.
        //{


        //    //var db = new WorkOfTimeDatabase();
        //    //var profil = db.GetSysFilesFilePathByDataTableAndFileGroupAndDataId("SH_User", "Profil Resmi", id);// dbden pp fotoları geliyor

        //    //if (profil == null || !System.IO.File.Exists(Server.MapPath(profil.FilePath)))// Profil reesmi olmayanlara boş profil resmi ataması yapılıyor.
        //    //{

        //    //    using (var ms = new MemoryStream())
        //    //    {
        //    //        var bitma = Bitmap.FromFile(Server.MapPath("/Content/custom/img/PersonsBackImage/nonprofilemark.png"));
        //    //        bitma.Save(ms, ImageFormat.Png);

        //    //        Response.Clear();
        //    //        Response.ContentType = "image/png";
        //    //        Response.AddHeader("content-disposition", "attachment; qr.png");
        //    //        Response.BinaryWrite(ms.ToArray());
        //    //        Response.End();

        //    //    }

        //    //    return Content(null);

        //    //}
    }
}

