using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infoline.OmixEntegrationApp.FtpEntegration.Utils
{
    public class Tools
    {
        public static bool IsDir(string value)
        {
            return value.Contains("<DIR>");
        }
        public static DateTime GetDate(string value)
        {
            var date = value.Substring(0, 17);
            return DateTime.Parse(date, CultureInfo.InvariantCulture);
        }
        public static string GetItemName(string line)
        {
            return line.Substring(39);
        }
        public static DateTime GetDateFromFileName(string fileName, string dateTimeFormat)
        {
            if (fileName.Contains("SELLIN"))
            {
                fileName = fileName.Substring(7);
                fileName = fileName.Split('.')[0];
                return DateTime.ParseExact(fileName, dateTimeFormat, CultureInfo.InvariantCulture);
            }
            else
            {
                fileName = fileName.Substring(8);
                fileName = fileName.Split('.')[0];
                return DateTime.ParseExact(fileName, dateTimeFormat, CultureInfo.InvariantCulture);
            }
        }
    }
}
