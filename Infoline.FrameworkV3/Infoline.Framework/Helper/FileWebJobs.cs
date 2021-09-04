using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Infoline.Framework.Helper
{
    public class FileWebJobs
    {
        private static string GetFileUpload(HttpContext context, string _filename)
        {
            HttpFileCollection files = context.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                file.SaveAs(@"D:\" + file.FileName.ToString());
                _filename += "[" + file.FileName + "]";
            }
            return _filename;
        }
      
        //File Upload Example
        /*
         string fileToUpload = @"c:\3G-GPRS-GPS-Arduino-Shield-With-Audio-Video-Kit.pdf";
            string url = "http://localhost:38567/MobilImzaBeyannameler/UploadPDF";
            using (var client = new WebClient())
            {
                byte[] result = client.UploadFile(url, fileToUpload);
                string responseAsString = Encoding.Default.GetString(result);
            }
         */

        public static string FileBoyut(long bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "TB","GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }
    }
}
