using Infoline.WorkOfTime.BusinessAccess;
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

namespace Infoline.WorkOfTime.WebProject.Controllers
{
    public class QRController : Controller
    {

        public class SignQrCore
        {
            public string WebServiceUrl { get; set; }
            public string WebUrl { get; set; }
        }

        [AllowEveryone]
        public ContentResult QRCodeCreative()
        {

            var signQrCore = new SignQrCore
            {
                WebServiceUrl = TenantConfig.Tenant.GetWebServiceUrl(),
                WebUrl = TenantConfig.Tenant.GetWebUrl()
            };
            var url = Infoline.Helper.Json.Serialize(signQrCore);

            using (var watermarkImage = QRCodeCreate(url, 0))
            {

                using (var ms = new MemoryStream())
                {

                    watermarkImage.Save(ms, ImageFormat.Bmp);


                    Response.Clear();
                    Response.ContentType = "image/png";
                    Response.AddHeader("content-disposition", "attachment; qr.png");
                    Response.BinaryWrite(ms.ToArray());
                    Response.End();



                }

            }

            return Content("");

        }
        public static System.Drawing.Image QRCodeCreate(string txt, int kkDuzey)
        {
            var _code = new QRCodeEncoder();
            _code.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            _code.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            _code.QRCodeVersion = kkDuzey;
            var bm = _code.Encode(txt);
            var newWidth = 100;
            var newHeight = 100;
            var resizedImage = new Bitmap(newWidth, newHeight);
            using (var gr = Graphics.FromImage(resizedImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(bm, new System.Drawing.Rectangle(0, 0, newWidth, newHeight));
            }
            return resizedImage;
        }

    }
}