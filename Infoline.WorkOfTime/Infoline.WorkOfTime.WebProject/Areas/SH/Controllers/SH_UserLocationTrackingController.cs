using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        

        [AllowEveryone]
        public ContentResult RedMark(Guid id)//// bool ccc ekleyip arka tarafa green resmini getir.
        {

            var db = new WorkOfTimeDatabase();
            var profil = db.GetSysFilesFilePathByDataTableAndFileGroupAndDataId("SH_User", "Profil Resmi", id);// dbden pp fotoları geliyor
            if (profil==null)// Profil reesmi olmayanlara boş profil resmi ataması yapılıyor.
            {
                profil = new SYS_Files
                {
                    FilePath = "/Content/Custom/img/na.png"
                };
            }

            var mark = new System.Drawing.Bitmap(Server.MapPath("/Content/Custom/img/PersonsBackImage/mark.png"));// markın bulunduğu alan
#if DEBUG

            var profilFoto = new System.Drawing.Bitmap(Server.MapPath(profil.FilePath));

            //Profil fotoğrafının circle yapıldığı yer
            Image dstImage = new Bitmap(profilFoto.Width, profilFoto.Height, profilFoto.PixelFormat);
            Graphics c = Graphics.FromImage(dstImage);
            using (Brush br = new SolidBrush(Color.White))
            {
                c.CompositingMode = CompositingMode.SourceCopy;
                c.CompositingQuality = CompositingQuality.HighQuality;
                c.InterpolationMode = InterpolationMode.HighQualityBicubic;
                c.SmoothingMode = SmoothingMode.HighQuality;
                c.PixelOffsetMode = PixelOffsetMode.HighQuality;
                c.FillRectangle(br, 0, 0, 40, 40);
            }
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(new Rectangle(0, 0, dstImage.Width, dstImage.Height));
            c.SetClip(path);
            c.DrawImage(profilFoto, 0, 0);

            // Markın üztüne profil fotoğrafının basıldığı yer
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mark))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(dstImage, new System.Drawing.Rectangle(12, 1, 40, 40));

            }




            using (var ms = new MemoryStream())
            {

                mark.Save(ms, ImageFormat.Png);// format değişince background siyah yerine oluşan imleç çıkıyor sadece

                Response.Clear();
                Response.ContentType = "image/png";
                Response.AddHeader("content-disposition", "attachment; qr.png");
                Response.BinaryWrite(ms.ToArray());
                Response.End();

            }

            return Content(null);


#endif
            var profilFotos = new System.Drawing.Bitmap(Server.MapPath(profil.FilePath));
            
            //  kalite bozulmadan resize
            //  var profil2 = new Bitmap(profilFoto, 40, 40);


            //Profil fotoğrafının circle yapıldığı yer
                Image dstImages = new Bitmap(profilFotos.Width, profilFotos.Height, profilFotos.PixelFormat);
                Graphics cs = Graphics.FromImage(dstImages);
                using (Brush br = new SolidBrush(Color.White))
                {
                cs.CompositingMode = CompositingMode.SourceCopy;
                cs.CompositingQuality = CompositingQuality.HighQuality;
                cs.InterpolationMode = InterpolationMode.HighQualityBicubic;
                cs.SmoothingMode = SmoothingMode.HighQuality;
                cs.PixelOffsetMode = PixelOffsetMode.HighQuality;
                cs.FillRectangle(br, 0, 0, 40, 40);
                }
                GraphicsPath paths = new GraphicsPath();
                paths.AddEllipse(new Rectangle(0, 0, dstImage.Width, dstImage.Height));
                cs.SetClip(paths);
                cs.DrawImage(profilFoto, 0, 0);
                
            // Markın üztüne profil fotoğrafının basıldığı yer
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mark))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(dstImage, new System.Drawing.Rectangle(12, 1, 40, 40));

            }



            using (var ms = new MemoryStream())
            {

                mark.Save(ms, ImageFormat.Png);// format değişince background siyah yerine oluşan imleç çıkıyor sadece

                Response.Clear();
                Response.ContentType = "image/png";
                Response.AddHeader("content-disposition", "attachment; qr.png");
                Response.BinaryWrite(ms.ToArray());
                Response.End();

            }

            return Content(null);

        }


        [AllowEveryone]
        public ContentResult GreenMark(Guid id)
        {

            var db = new WorkOfTimeDatabase();
            var profil = db.GetSysFilesFilePathByDataTableAndFileGroupAndDataId("SH_User", "Profil Resmi", id);// dbden pp fotoları geliyor


            var GreenMark = new System.Drawing.Bitmap(Server.MapPath("/Content/Custom/img/PersonsBackImage/GreenMark.png"));// markın bulunduğu alan

#if DEBUG
            var profilFoto = new System.Drawing.Bitmap(Server.MapPath(profil.FilePath));

            //  kalite bozulmadan resize
            //  var profil2 = new Bitmap(profilFoto, 40, 40);


            //Profil fotoğrafının circle yapıldığı yer
            Image dstImage = new Bitmap(profilFoto.Width, profilFoto.Height, profilFoto.PixelFormat);
            Graphics c = Graphics.FromImage(dstImage);
            using (Brush br = new SolidBrush(Color.White))
            {
                c.CompositingMode = CompositingMode.SourceCopy;
                c.CompositingQuality = CompositingQuality.HighQuality;
                c.InterpolationMode = InterpolationMode.HighQualityBicubic;
                c.SmoothingMode = SmoothingMode.HighQuality;
                c.PixelOffsetMode = PixelOffsetMode.HighQuality;
                c.FillRectangle(br, 0, 0, 40, 40);
            }
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(new Rectangle(0, 0, dstImage.Width, dstImage.Height));
            c.SetClip(path);
            c.DrawImage(profilFoto, 0, 0);

            // Markın üztüne profil fotoğrafının basıldığı yer
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(GreenMark))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(dstImage, new System.Drawing.Rectangle(12, 1, 40, 40));

            }




            using (var ms = new MemoryStream())
            {

                GreenMark.Save(ms, ImageFormat.Png);// format değişince background siyah yerine oluşan imleç çıkıyor sadece

                Response.Clear();
                Response.ContentType = "image/png";
                Response.AddHeader("content-disposition", "attachment; qr.png");
                Response.BinaryWrite(ms.ToArray());
                Response.End();

            }

            return Content(null);
#endif
            var profilFotos = new System.Drawing.Bitmap(Server.MapPath(profil.FilePath));

            //  kalite bozulmadan resize
            //  var profil2 = new Bitmap(profilFoto, 40, 40);


            //Profil fotoğrafının circle yapıldığı yer
            Image dstImages = new Bitmap(profilFotos.Width, profilFotos.Height, profilFotos.PixelFormat);
            Graphics cs = Graphics.FromImage(dstImages);
            using (Brush br = new SolidBrush(Color.White))
            {
                cs.CompositingMode = CompositingMode.SourceCopy;
                cs.CompositingQuality = CompositingQuality.HighQuality;
                cs.InterpolationMode = InterpolationMode.HighQualityBicubic;
                cs.SmoothingMode = SmoothingMode.HighQuality;
                cs.PixelOffsetMode = PixelOffsetMode.HighQuality;
                cs.FillRectangle(br, 0, 0, 40, 40);
            }
            GraphicsPath paths = new GraphicsPath();
            paths.AddEllipse(new Rectangle(0, 0, dstImage.Width, dstImage.Height));
            c.SetClip(paths);
            c.DrawImage(profilFotos, 0, 0);

            // Markın üztüne profil fotoğrafının basıldığı yer
            using (System.Drawing.Graphics gs = System.Drawing.Graphics.FromImage(GreenMark))
            {
                gs.CompositingQuality = CompositingQuality.HighQuality;
                gs.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gs.SmoothingMode = SmoothingMode.HighQuality;
                gs.DrawImage(dstImage, new System.Drawing.Rectangle(12, 1, 40, 40));

            }




            using (var ms = new MemoryStream())
            {

                GreenMark.Save(ms, ImageFormat.Png);// format değişince background siyah yerine oluşan imleç çıkıyor sadece

                Response.Clear();
                Response.ContentType = "image/png";
                Response.AddHeader("content-disposition", "attachment; qr.png");
                Response.BinaryWrite(ms.ToArray());
                Response.End();

            }

            return Content(null);
        }



    }
}
