using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class CalculateReturn
    {
        public double? TotalHour { get; set; }
        public double? TotalDay { get; set; }
        public string Text { get; set; }
        public DateTime? CommencementDate { get; set; }
    }
    public static class INV_PermitCalculator
    {
        public static Guid yillikIzin = new Guid("F8547488-3215-1235-5126-EF2323CBBCB2");

        public static Guid mazeretIzin = new Guid("AF40ADF4-9963-4790-A9DE-1D74C32B61C1");
        public static Guid avansIzin = new Guid("ABFC9795-6624-4805-B29B-9EE3D7F25905");

        public static INV_PermitOffical[] officalPermit = checkOfficialPermit(new WorkOfTimeDatabase().GetINV_PermitOffical());

        //  public static INV_PermitType[] permitType = new WorkOfTimeDatabase().GetINV_PermitType();
        public static INV_PermitOffical[] checkOfficialPermit(INV_PermitOffical[] officalPermitm)
        {
            foreach (var offical in officalPermitm)
            {
                if (offical.StartDate.HasValue)
                {
                    var newstart = new DateTime(offical.StartDate.Value.Ticks);
                    var start = TenantConfig.Tenant.Config.WorkingTimes[newstart.DayOfWeek].allowTimes.OrderBy(a => a.Start).Select(a => a.Start).FirstOrDefault();
                    offical.StartDate = new DateTime(newstart.Year, newstart.Month, newstart.Day, start.Hours, start.Minutes, 0);
                }

                if (offical.EndDate.HasValue)
                {
                    var newend = new DateTime(offical.EndDate.Value.Ticks);
                    var end = TenantConfig.Tenant.Config.WorkingTimes[newend.DayOfWeek].allowTimes.OrderBy(a => a.End).Reverse().Select(a => a.End).FirstOrDefault();
                    offical.EndDate = new DateTime(newend.Year, newend.Month, newend.Day, end.Hours, end.Minutes, 0);
                }
            }

            return officalPermitm;
        }
        private static DateTime? CommencementDate(INV_Permit item)
        {
            DateTime returnDate = new DateTime(item.EndDate.Value.Ticks);

            do
            {
                var timespan = new TimeSpan(returnDate.Hour, returnDate.Minute, returnDate.Second);
                var current = TenantConfig.Tenant.Config.WorkingTimes[returnDate.DayOfWeek];
                var offical = officalPermit.Where(a => a.StartDate <= returnDate && a.EndDate >= returnDate);

                if (offical.Count() == 0 && current.isWorking && current.allowTimes.Count(a => a.Start <= timespan && a.End > timespan) > 0)
                {
                    break;
                }

                returnDate = returnDate.AddMinutes(15);
            } while (true);


            return returnDate;
        }
        public static DateTime CommencementDate(DateTime date)
        {
            return CommencementDate(new INV_Permit { EndDate = date }).Value;
        }
        public static CalculateReturn Calculate(INV_Permit item)
        {
            if (item.StartDate == null || item.EndDate == null)
            {
                return new CalculateReturn { Text = "false" };
            }

            DateTime startDate = new DateTime(item.StartDate.Value.Ticks);
            DateTime endDate = new DateTime(item.EndDate.Value.Ticks);
            INV_PermitType type = null;
            if (item.PermitTypeId.HasValue)
            {
                type = new WorkOfTimeDatabase().GetINV_PermitTypeById(item.PermitTypeId.Value);
            }
            var result = new CalculateReturn { CommencementDate = CommencementDate(item), TotalDay = 0, TotalHour = 0 };

            do
            {
                if (startDate >= endDate)
                {
                    break;
                }

                var timespan = new TimeSpan(startDate.Hour, startDate.Minute, startDate.Second);
                var current = TenantConfig.Tenant.Config.WorkingTimes[startDate.DayOfWeek];
                var offical = officalPermit.Where(a => a.StartDate <= startDate && a.EndDate >= startDate);

                if (type != null && type.CanHourly == true)
                {
                    if (offical.Count() == 0 && current.isWorking && current.allowTimes.Count(a => a.Start <= timespan && a.End > timespan) > 0)
                    {
                        result.TotalHour = result.TotalHour + 0.25;


                    }
                    startDate = startDate.AddMinutes(15);
                }
                else
                {
                    if (offical.Count() == 0 && current.isWorking)
                    {
                        result.TotalDay = result.TotalDay + 1;
                    }
                    startDate = startDate.AddDays(1);
                }
            } while (true);


            if (type != null && type.CanHourly == false)
            {
                result.TotalHour = result.TotalDay * 9;
            }
            else
            {
                result.TotalDay = result.TotalHour / 9;
            }


            var minute = result.TotalHour.Value % 1;
            var day = Math.Floor(result.TotalHour.Value / 9);
            var hour = Math.Floor(result.TotalHour.Value - (day * 9));

            result.Text += day > 0 ? day + " Gün " : "";
            result.Text += hour > 0 ? hour + " Saat " : "";
            result.Text += minute > 0 ? (minute * 60) + " Dakika " : "";
            return result;
        }

        public static CalculateReturn CalculateAll(INV_Permit item)
        {
            if (item.StartDate == null || item.EndDate == null)
            {
                return new CalculateReturn { Text = "false" };
            }

            DateTime startDate = new DateTime(item.StartDate.Value.Ticks);
            DateTime endDate = new DateTime(item.EndDate.Value.Ticks);
            INV_PermitType type = null;
            if (item.PermitTypeId.HasValue)
            {
                type = new WorkOfTimeDatabase().GetINV_PermitTypeById(item.PermitTypeId.Value);
            }
            var result = new CalculateReturn { CommencementDate = CommencementDate(item), TotalDay = 0, TotalHour = 0 };

            do
            {
                if (startDate >= endDate)
                {
                    break;
                }

                var timespan = new TimeSpan(startDate.Hour, startDate.Minute, startDate.Second);
                var current = TenantConfig.Tenant.Config.WorkingTimes[startDate.DayOfWeek];

                if (type != null && type.CanHourly == true)
                {
                    if (current.isWorking && current.allowTimes.Count(a => a.Start <= timespan && a.End > timespan) > 0)
                    {
                        result.TotalHour = result.TotalHour + 0.25;
                    }
                    startDate = startDate.AddMinutes(15);
                }
                else
                {
                    if (current.isWorking)
                    {
                        result.TotalDay = result.TotalDay + 1;
                    }
                    startDate = startDate.AddDays(1);
                }
            } while (true);


            if (type != null && type.CanHourly == false)
            {
                result.TotalHour = result.TotalDay * 9;
            }
            else
            {
                result.TotalDay = result.TotalHour / 9;
            }


            var minute = result.TotalHour.Value % 1;
            var day = Math.Floor(result.TotalHour.Value / 9);
            var hour = Math.Floor(result.TotalHour.Value - (day * 9));

            result.Text += day > 0 ? day + " Gün " : "";
            result.Text += hour > 0 ? hour + " Saat " : "";
            result.Text += minute > 0 ? (minute * 60) + " Dakika " : "";

            return result;
        }


        public static CalculateReturn Calculate(VWINV_Permit item)
        {
            return Calculate(new INV_Permit().B_EntityDataCopyForMaterial(item, true));
        }
        public static ResultStatus Validate(INV_Permit item, VWSH_User user, VWSH_User insanKaynaklariUser)
        {
            var db = new WorkOfTimeDatabase();
            INV_PermitType type = null;
            if (item.PermitTypeId.HasValue)
            {
                type = new WorkOfTimeDatabase().GetINV_PermitTypeById(item.PermitTypeId.Value);
            }
            var result = new ResultStatus
            {
                result = false,
            };

            if (type == null)
            {
                result.message = "İzin tipi bulunamadı.";
                return result;
            }

            if ((type.CanHourly == true && (item.TotalHours == null || item.TotalHours == 0)) || (type.CanHourly == false && (item.TotalDays == null || item.TotalDays == 0)))
            {
                result.message = "Seçtiğiniz başlangıç ve bitiş tarihleri'ni kontrol ediniz.";
                return result;
            }

            //Tanımlı izinler
            if (type.PaidPermitDay.HasValue && item.TotalDays > type.PaidPermitDay.Value)
            {
                result.message = string.Format("{0} izni maksimum {1} gündür.{2} günlük tarih aralığı seçtiniz tarih aralığınız kontrol ediniz.", type.Name, type.PaidPermitDay.Value, item.TotalDays);
                return result;

            }

            if (item.StartDate >= DateTime.Now)
            {

                var permitsStandBy = db.GetVWINV_PermitStandByByIdUser(item.IdUser.Value);
                if (permitsStandBy.Count() > 3)
                {
                    result.message = string.Format("Personelin onayda bekleyen izin talepleri bulunmaktadır.Bu talepler cevaplanmadan yeni talep'de bulunulamaz.");
                    return result;
                }

                var controlCommission = db.GetVWINV_CommissionsPersonsByControlDate(item.IdUser.Value, item.StartDate.Value, item.EndDate.Value);
                if (controlCommission != null)
                {
                    result.message = string.Format("{0} ile {1} tarihleri arasında göreviniz bulunmaktadır. Farklı bir tarih aralığını seçerek tekrar deneyiniz.", string.Format("{0:dd.MM.yyyy HH:mm}", controlCommission.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", controlCommission.EndDate));
                    return result;
                }

                var controlPermit = db.GetINV_PermitByControlDate(item.IdUser.Value, item.StartDate.Value, item.EndDate.Value);
                if (controlPermit != null)
                {
                    result.message = string.Format("{0} ile {1} tarihleri arasında zaten izin talebiniz bulunmaktadır.", string.Format("{0:dd.MM.yyyy HH:mm}", controlPermit.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", controlPermit.EndDate));
                    return result;
                }
            }
            else
            {

                var controlPermit = db.GetINV_PermitApprovedPermit(item.IdUser.Value, item.StartDate.Value, item.EndDate.Value);
                if (controlPermit != null)
                {
                    result.message = string.Format("Personelin {0} ile {1} tarihleri arasında zaten kayıtlı izni bulunmaktadır.", string.Format("{0:dd.MM.yyyy HH:mm}", controlPermit.StartDate), string.Format("{0:dd.MM.yyyy HH:mm}", controlPermit.EndDate));
                    return result;
                }
            }

            //IK GELECEĞE AVANS İZİN GİRMEK İSTERSE NİYE DİYE SORMUYORUZ İZİN VERİYORUZ (Tuba Hanımın özel isteği üzerine)
            if (/*item.StartDate > DateTime.Now && */insanKaynaklariUser != null && insanKaynaklariUser.id == item.createdby)
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Başarılı"
                };
            }


            /*İzin Müsaitlik validasyon*/

            if(item.PermitTypeId == avansIzin)
            {
                var timeSpan = item.StartDate - user.JobStartDate;
                var totalYear = timeSpan.Value.TotalDays / 365;
                var totalPermit = 0;
                for (int i = 1; i < totalYear; i++)
                {
                    totalPermit += (i < 6 ? 14 : (i < 15 ? 20 : 26));
                }

                if (totalPermit > 0)
                {
                    result.message = "Yıllık izin hakkınız bulunduğundan avans izin talebi gerçekleştiremezsiniz.";
                    return result;
                }
            }

            if (item.PermitTypeId == yillikIzin)
            {

                var timeSpan = item.StartDate - user.JobStartDate;
                var totalYear = timeSpan.Value.TotalDays / 365;
                var totalPermit = 0;
                for (int i = 1; i < totalYear; i++)
                {
                    totalPermit += (i < 6 ? 14 : (i < 15 ? 20 : 26));
                }

                if (totalPermit > 0)
                {
                    if (item.TotalDays > user.PermitYearlyUsable)
                    {
                        result.message = string.Format("Personelin kalan yıllık izin hakkı {1} gün'dür.Almaya çalışılan izin {2} gündür ve kalan izin hakkı bu izin süresini karşılamamaktadır.", string.Format("{0:dd.MM.yyyy HH:mm}", item.StartDate), user.PermitYearlyUsable, item.TotalDays);
                        return result;
                    }

                }
                else
                {
                    result.message = string.Format("{0} tarihinde personel'in yıllık izin hakkı bulunmamaktadır.", string.Format("{0:dd.MM.yyyy HH:mm}", item.StartDate));
                    return result;
                }
            }

            /*İzin Müsaitlik Validasyon*/



            if (item.PermitTypeId == mazeretIzin)
            {

                if (user.PermitYearlyUsable > 0 && item.TotalHours >= 8.5)
                {
                    result.message = string.Format("Personelin yıllık izin hakkı bulunduğundan tam gün mazeret izni kullanamaz.Yıllık izin olarak talepte bulununuz.");
                    return result;

                }
                if (item.TotalHours > user.PermitExcuseUsable)
                {
                    result.message = string.Format("Personelin {0} yılı için kalan mazeret izin hakkı {1} saat'dir.Almaya çalışılan izin {2} saatdir ve kalan izin hakkı bu izin süresini karşılamamaktadır.", item.StartDate.Value.Year, user.PermitExcuseUsable, item.TotalHours);
                    return result;
                }
            }

            return new ResultStatus
            {
                result = true,
                message = "Başarılı"
            };

        }

        public static string FormatHour(double? hour)
        {
            if (hour == null || hour == 0)
            {
                return "0 saat";
            }

            if (hour == null || hour == 0)
            {
                return "0 saat";
            }

            var mntUsed = hour % 1;
            var minuteUsed = Math.Floor((60 * mntUsed.Value));
            var hourUsed = Math.Floor(hour.Value);
            var txtUsed = "";
            txtUsed += hourUsed != 0 ? hourUsed + " saat " : "";
            txtUsed += minuteUsed != 0 ? minuteUsed + " dakika" : "";
            return txtUsed;
        }

    }
}
