using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        [AllowEveryone]
        public static System.Drawing.Bitmap CombineBitmap(string[] files)
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string greenbackground in files)
                {
                    //create a Bitmap from the file and add it to the list
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(greenbackground);

                    //update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Red);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (System.Drawing.Bitmap redbackground in images)
                    {
                        g.DrawImage(redbackground,
                          new System.Drawing.Rectangle(offset, 0, redbackground.Width, redbackground.Height));
                        offset += redbackground.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap redbackground in images)
                {
                    redbackground.Dispose();
                }
            }
        }

    }
}