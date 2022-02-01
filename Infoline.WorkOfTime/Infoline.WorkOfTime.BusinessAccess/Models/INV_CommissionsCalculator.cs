using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public static class INV_CommissionsCalculator
    {
        public class CalculateReturn
        {
            public double? TotalHour { get; set; }
            public double? TotalDay { get; set; }
            public string Text { get; set; }
            public DateTime? CommencementDate { get; set; }
        }
        private static DateTime? CommencementDate(INV_Commissions item)
        {
            DateTime returnDate = new DateTime(item.EndDate.Value.Ticks);

            do
            {
                var timespan = new TimeSpan(returnDate.Hour, returnDate.Minute, returnDate.Second);
                var current = TenantConfig.Tenant.Config.WorkingTimes[returnDate.DayOfWeek];

                if (current.isWorking && current.allowTimes.Count(a => a.Start <= timespan && a.End > timespan) > 0)
                {
                    break;
                }

                returnDate = returnDate.AddMinutes(15);
            } while (true);


            return returnDate;
        }
        public static DateTime CommencementDate(DateTime date)
        {
            return CommencementDate(new INV_Commissions { EndDate = date }).Value;
        }
        public static CalculateReturn Calculate(INV_Commissions item)
        {

            DateTime startDate = new DateTime(item.StartDate.Value.Ticks);
            DateTime endDate = new DateTime(item.EndDate.Value.Ticks);


            DateTime bTarih = endDate;
            DateTime kTarih = startDate;
            TimeSpan Sonuc = bTarih - kTarih;
            if (Sonuc<new TimeSpan(-1,0,0))
            {
                return new CalculateReturn { CommencementDate = CommencementDate(item), TotalDay = 0, TotalHour = 0, Text = "0 Gün" };

            }
            string str = "";
            var gun = Math.Floor(Sonuc.TotalMinutes / (60 * 24));
            if (gun != 0)
            {
                str += gun + " gün ";
            }
            var saat = Math.Floor(Sonuc.TotalMinutes % (60 * 24) / 60);
            if (saat != 0)
            {
                str += saat + " saat ";
            }
            var dakika = Math.Floor(Sonuc.TotalMinutes % (60 * 24) - saat * 60);
            if (dakika != 0)
            {
                str += dakika + " dakika";
            }

            var result = new CalculateReturn { CommencementDate = CommencementDate(item), TotalDay = 0, TotalHour = 0 };

            result.TotalHour = saat > 0 ? saat : 0;
            result.TotalDay = gun > 0 ? gun : 0;
            result.Text += str;

            return result;
        }
        public static CalculateReturn Calculate(VWINV_Commissions item)
        {
            return Calculate(new INV_Commissions().B_EntityDataCopyForMaterial(item, true));
        }

    }
}
