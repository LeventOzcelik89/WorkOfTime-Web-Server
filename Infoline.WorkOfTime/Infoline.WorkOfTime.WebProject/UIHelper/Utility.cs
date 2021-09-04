using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebProject.UIHelper
{
    public class Utility
    {
        public class DateFunctions
        {
            public DateFunctions()
            {
                BuYilBasi = new DateTime(DateTime.Now.Year, 1, 1);
                AyBasi = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                HaftaBasi = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
                BuHaftaSonu = DateTime.Now.AddDays((7 - (int)DateTime.Now.DayOfWeek));
                AySonu = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }

            public DateTime BuYilBasi { get; set; }
            public DateTime AyBasi { get; set; }
            public DateTime HaftaBasi { get; set; }
            public DateTime BuHaftaSonu { get; set; }
            public DateTime AySonu { get; set; }

        }
    }
}