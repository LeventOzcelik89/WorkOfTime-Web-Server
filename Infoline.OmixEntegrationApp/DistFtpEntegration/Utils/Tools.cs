using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Utils
{
    public class Tools
    {
        public static bool IsDir(string value)
        {
            
            return value.Contains("<DIR>");
            
        }
        public static DateTime GetDate( string value)
        {
            var date = value.Substring(0, 17);
            return DateTime.Parse(date, CultureInfo.InvariantCulture);
        }

        public static string GetItemName(string line)
        {
            return line.Substring(39);
        }
    }
}
