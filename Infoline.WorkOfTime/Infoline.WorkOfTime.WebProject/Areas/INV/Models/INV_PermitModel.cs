using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Models
{
    public class INV_PermitModel
    {
        public ChartINV_Permit[] allPermits(VWINV_Permit[] permit)
        {
            try
            {
                var izin = new List<ChartINV_Permit>();

                for (int i = 1; i <= 12; i++)
                {
                    izin.Add(new ChartINV_Permit
                    {

                        Ay = (DateTime.Now.Month >= i
                            ? DateTime.Now.Year
                            : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),


                        tumIzinler = permit.Where(b => (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.GecmisIzin) && b.StartDate.Value.Month == i && b.StartDate.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count(),


                        ToplamGun = permit.Where(b => (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.GecmisIzin) && b.StartDate.Value.Month == i && b.StartDate.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Sum(a => a.TotalDays).Value

                    });
                }
                var gecmisYillar = izin.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = izin.Take(DateTime.Now.Month).ToList();
                izin = gecmisYillar;
                izin.AddRange(mevcutYillar);

                return izin.ToArray();
            }
            catch (Exception)
            {
                return new ChartINV_Permit[] { };
            }

        }

        public ChartINV_Permit[] allPermitsCurrentlyYear(VWINV_Permit[] permit)
        {
            try
            {
                var izin = new List<ChartINV_Permit>();
                for (int i = 1; i <= 12; i++)
                {
                    izin.Add(new ChartINV_Permit
                    {
                        Ay = (DateTime.Now.Month <= i ? DateTime.Now.Year : (DateTime.Now.Year)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                        tumIzinler = permit.Where(b => b.StartDate.Value.Month == i && b.StartDate.Value.Year == DateTime.Now.Year && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.GecmisIzin)).Count(),
                        //  ToplamGun = permit.Where(b => b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay && b.StartDate.Value.Month == i && b.StartDate.Value.Year == DateTime.Now.Year).Sum(x => x.PermissionDay).Value
                    });
                }
                var gecmisYillar = izin.Skip(DateTime.Now.Month).Take(12).ToList();
                var mevcutYillar = izin.Take(DateTime.Now.Month).ToList();
                izin.AddRange(mevcutYillar);
                izin.AddRange(gecmisYillar);
                return izin.ToArray();
            }
            catch (Exception)
            {
                return new ChartINV_Permit[] { };
            }

        }

        public List<ChartKeyVWINV_Permit> allPermitsByDate(VWINV_Permit[] permit)
        {
            try
            {
                var gunBasi = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var haftaBasi = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
                var ayBasi = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var seneBasi = new DateTime(DateTime.Now.Year, 1, 1);

                var permitList = new List<ChartKeyVWINV_Permit>();
                var daily = permit.Where(x => x.StartDate >= gunBasi && x.StartDate <= DateTime.Now
                                        && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Gunluk", Permits = daily });

                var weekly = permit.Where(x => x.StartDate >= haftaBasi && x.StartDate <= DateTime.Now
                                        && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Haftalik", Permits = weekly });

                var mounthly = permit.Where(x => x.StartDate >= ayBasi && x.StartDate <= DateTime.Now
                                        && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Aylik", Permits = mounthly });

                var yearly = permit.Where(x => x.StartDate >= seneBasi && x.StartDate <= DateTime.Now
                                        && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Yillik", Permits = yearly });


                return permitList;
            }
            catch (Exception)
            {
                return new List<ChartKeyVWINV_Permit>();
            }

        }

        public List<ChartKeyVWINV_Permit> allPermitsFutureByDate(VWINV_Permit[] permit)
        {
            try
            {
                var yil = DateTime.Now.Year;
                var ay = DateTime.Now.Month;
                var gun = DateTime.Now.Day;
                var gunSonu = new DateTime(yil, ay, gun, 23, 59, 59);
                var buHaftaSonu = DateTime.Now.AddDays((7 - (int)DateTime.Now.DayOfWeek));
                var aySonu = new DateTime(yil, ay, DateTime.DaysInMonth(yil, ay));
                var seneSonu = new DateTime(yil, 12, 31);

                var permitList = new List<ChartKeyVWINV_Permit>();
                var daily = permit.Where(x => x.StartDate > DateTime.Now && x.StartDate < gunSonu
                                        && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Gunluk", Permits = daily });

                var weekly = permit.Where(x => x.StartDate >= DateTime.Now && x.StartDate <= buHaftaSonu
                                        && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Haftalik", Permits = weekly });

                var mounthly = permit.Where(x => x.StartDate >= DateTime.Now && x.StartDate <= aySonu
                                        && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Aylik", Permits = mounthly });

                var yearly = permit.Where(x => x.StartDate >= DateTime.Now && x.StartDate <= seneSonu
                                        && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Yillik", Permits = yearly });


                return permitList;
            }
            catch (Exception)
            {
                return new List<ChartKeyVWINV_Permit>();
            }

        }

        public List<ChartKeyValueDesc[]> PermitPosition(VWINV_Permit[] permit)
        {
            try
            {
                var gunBasi = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var haftaBasi = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
                var ayBasi = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var seneBasi = new DateTime(DateTime.Now.Year, 1, 1);

                var list = new List<ChartKeyValueDesc[]>();
                var permitByTypeDaily = permit.GroupBy(x => x.PermitType_Title).Select(x => new ChartKeyValueDesc
                {
                    Key = x.Key,
                    Value = x.Where(b => b.StartDate >= gunBasi && b.StartDate <= DateTime.Now && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Count(),
                    Description = "Gunluk",
                    TotalDays = x.Where(b => b.StartDate >= gunBasi && b.StartDate <= DateTime.Now && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Select(b => b.TotalDays).Sum().Value
                }).Where(x => x.Value > 0).OrderByDescending(c => c.Value).ToArray().ToOrderByDescColorGreen();
                list.Add(permitByTypeDaily);
                var permitByTypeWeekly = permit.GroupBy(x => x.PermitType_Title).Select(x => new ChartKeyValueDesc
                {
                    Key = x.Key,
                    Description = "Haftalik",
                    TotalDays = x.Where(b => b.StartDate >= haftaBasi && b.StartDate <= DateTime.Now && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Select(b => b.TotalDays).Sum().Value,
                    Value = x.Where(b => b.StartDate >= haftaBasi && b.StartDate <= DateTime.Now && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Count()
                }).Where(x => x.Value > 0).OrderByDescending(c => c.Value).ToArray().ToOrderByDescColorGreen();
                list.Add(permitByTypeWeekly);
                var permitByTypeMonthly = permit.GroupBy(x => x.PermitType_Title).Select(x => new ChartKeyValueDesc
                {
                    Key = x.Key,
                    Description = "Aylik",
                    TotalDays = x.Where(b => b.StartDate >= ayBasi && b.StartDate <= DateTime.Now && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Select(b => b.TotalDays).Sum().Value,
                    Value = x.Where(b => b.StartDate >= ayBasi && b.StartDate <= DateTime.Now && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Count()
                }).Where(x => x.Value > 0).OrderByDescending(c => c.Value).ToArray().ToOrderByDescColorGreen();
                list.Add(permitByTypeMonthly);
                var permitByTypeYearly = permit.GroupBy(x => x.PermitType_Title).Select(x => new ChartKeyValueDesc
                {
                    Key = x.Key,
                    Description = "Yillik",
                    TotalDays = x.Where(b => b.StartDate >= seneBasi && b.StartDate <= DateTime.Now && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Select(b => b.TotalDays).Sum().Value,
                    Value = x.Where(b => b.StartDate >= seneBasi && b.StartDate <= DateTime.Now && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Count()
                }).Where(x => x.Value > 0).OrderByDescending(c => c.Value).ToArray().ToOrderByDescColorGreen();
                list.Add(permitByTypeYearly);
                return list;
            }
            catch (Exception)
            {
                return new List<ChartKeyValueDesc[]>();
            }
        }

        public List<ChartKeyValueDesc[]> PermitFuturePosition(VWINV_Permit[] permit)
        {
            try
            {
                var yil = DateTime.Now.Year;
                var ay = DateTime.Now.Month;
                var gun = DateTime.Now.Day;
                var gunBasi = new DateTime(yil, ay, gun);
                var gunSonu = new DateTime(yil, ay, gun, 23, 59, 59);
                var buHaftaSonu = DateTime.Now.AddDays((7 - (int)DateTime.Now.DayOfWeek));
                var aySonu = new DateTime(yil, ay, DateTime.DaysInMonth(yil, ay));
                var seneSonu = new DateTime(yil, 12, 31);

                var list = new List<ChartKeyValueDesc[]>();
                var permitByTypeDaily = permit.GroupBy(x => x.PermitType_Title).Select(x => new ChartKeyValueDesc
                {
                    Key = x.Key,
                    Value = x.Where(b => b.StartDate.Value > DateTime.Now && b.StartDate <= gunSonu && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Count(),
                    Description = "Gunluk",
                    TotalDays = x.Where(b => b.StartDate.Value > DateTime.Now && b.StartDate <= gunSonu && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Select(b => b.TotalDays).Sum().Value
                }).Where(x => x.Value > 0).OrderByDescending(c => c.Value).ToArray().ToOrderByDescColorGreen();
                list.Add(permitByTypeDaily);
                var permitByTypeWeekly = permit.GroupBy(x => x.PermitType_Title).Select(x => new ChartKeyValueDesc
                {
                    Key = x.Key,
                    Description = "Haftalik",
                    TotalDays = x.Where(b => b.StartDate >= DateTime.Now && b.StartDate <= buHaftaSonu && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Sum(b => b.TotalDays.Value),
                    Value = x.Where(b => b.StartDate >= DateTime.Now && b.StartDate <= buHaftaSonu && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Count()
                }).Where(x => x.Value > 0).OrderByDescending(c => c.Value).ToArray().ToOrderByDescColorGreen();
                list.Add(permitByTypeWeekly);
                var permitByTypeMonthly = permit.GroupBy(x => x.PermitType_Title).Select(x => new ChartKeyValueDesc
                {
                    Key = x.Key,
                    Description = "Aylik",
                    TotalDays = x.Where(b => b.StartDate >= DateTime.Now && b.StartDate <= aySonu && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Select(b => b.TotalDays).Sum().Value,
                    Value = x.Where(b => b.StartDate >= DateTime.Now && b.StartDate <= aySonu && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Count()
                }).Where(x => x.Value > 0).OrderByDescending(c => c.Value).ToArray().ToOrderByDescColorGreen();
                list.Add(permitByTypeMonthly);
                var permitByTypeYearly = permit.GroupBy(x => x.PermitType_Title).Select(x => new ChartKeyValueDesc
                {
                    Key = x.Key,
                    Description = "Yillik",
                    TotalDays = x.Where(b => b.StartDate >= DateTime.Now && b.StartDate <= seneSonu && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Where(f => f.TotalDays.HasValue).Select(b => b.TotalDays.Value).Sum(),
                    Value = x.Where(b => b.StartDate >= DateTime.Now && b.StartDate <= seneSonu && (b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || b.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Count()
                }).Where(x => x.Value > 0).OrderByDescending(c => c.Value).ToArray().ToOrderByDescColorGreen();
                list.Add(permitByTypeYearly);
                return list;
            }
            catch
            {
                return new List<ChartKeyValueDesc[]>();
            }
        }

        public List<ChartKeyVWINV_Permit> PermitPersonListNewJobSearch(VWINV_Permit[] permit)
        {
            try
            {
                var permitList = new List<ChartKeyVWINV_Permit>();
                var ayBasi = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var aySonu = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                var yeniIsIzinTalebi = permit
                    .Where(x =>
                        (x.PermitTypeId == new Guid("9820815C-3FB4-40FD-8251-95235510710F") || x.PermitTypeId == new Guid("294AE861-6616-4D42-8D58-867DE6535E7B")) &&
                        x.StartDate >= ayBasi &&
                        x.StartDate <= aySonu &&
                        (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)
                    )
                    .ToArray();

                permitList.Add(new ChartKeyVWINV_Permit { Key = "YeniIs", Permits = yeniIsIzinTalebi });

                return permitList;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public KeyValue[] allDayPermit(VWINV_Permit[] permit)
        {
            try
            {
                DateTime tarih1 = Convert.ToDateTime(permit.Select(x => x.StartDate));
                DateTime tarih2 = Convert.ToDateTime(permit.Select(x => x.EndDate));
                for (; tarih1 < tarih2; tarih1 = tarih1.AddDays(1))
                {
                    if (tarih1.DayOfWeek != DayOfWeek.Sunday || tarih1.DayOfWeek != DayOfWeek.Saturday)
                    {
                        //Response.Write(tarih1.DayOfWeek.ToString());
                    }
                }
                var sonuc = permit.Select(x => new KeyValue
                {
                    Key = x.StartDate.Value.Day.ToString(),
                    Value = x.StartDate
                }).ToArray();

                return sonuc;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public KeyValue[] allPersonYearlyPermit(VWSH_User[] user)
        {
            try
            {
                var data = user.Where(c => c.PermitYearlyUsable > 0).Select(x => new KeyValue
                {
                    Key = x.Company_Title,
                    Color = x.firstname + " " + x.lastname,
                    Value = x.PermitYearlyUsable,
                    Tooltip = x.code
                }).OrderByDescending(c => c.Value).ToArray();

                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ChartKeyVWINV_Permit> PermitPersonListMarried(VWINV_Permit[] permit)
        {
            try
            {
                var permitList = new List<ChartKeyVWINV_Permit>();
                var seneBasi = new DateTime(DateTime.Now.Year, 1, 1);
                var seneSonu = new DateTime(DateTime.Now.Year, 12, 31);
                var gecmisEvlilik = permit.Where(x => x.PermitTypeId == new Guid("775E9350-E203-4F14-AEF8-7D76971873C5")
                                                    && x.StartDate >= seneBasi && x.StartDate <= DateTime.Now).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Gecmis", Permits = gecmisEvlilik });
                var gelecekEvlilik = permit.Where(x => x.PermitTypeId == new Guid("775E9350-E203-4F14-AEF8-7D76971873C5")
                                                    && x.StartDate >= DateTime.Now && x.StartDate <= seneSonu).ToArray();
                permitList.Add(new ChartKeyVWINV_Permit { Key = "Gelecek", Permits = gelecekEvlilik });
                return permitList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public class ChartINV_Permit
        {
            public string Ay { get; set; }
            public int tumIzinler { get; set; }
            public double ToplamGun { get; set; }
        }

        public class ChartKeyVWINV_Permit
        {
            public string Key { get; set; }
            public VWINV_Permit[] Permits { get; set; }
        }

        public class ChartKeyValueDesc
        {
            public string Key { get; set; }
            public int Value { get; set; }
            public double TotalDays { get; set; }
            public string Description { get; set; }
            public string Color { get; set; }
        }
    }

    public partial class VWINV_PermitPermissionIndex
    {
        public double ToplamTalep { get; set; }
        public double KullanilanIzin { get; set; }
        public double OnayBekleyen { get; set; }
        public double Reddedilen { get; set; }
    }

    public partial class VWINV_PermitDashboardPageReport
    {
        public int BuYilTalepEdilen { get; set; }
        public int BuYilOnaylanan { get; set; }
        public int BuYilReddedilen { get; set; }
        public int BuYilOnayBekleyenIzinler { get; set; }

    }

    public partial class VWINV_PermitIndexAllPageReport
    {
        public int BirinciYoneticiOnayiBekleyen { get; set; }
        public int IkinciYoneticiOnayiBekleyen { get; set; }
        public int IkOnayiBekleyen { get; set; }
        public int IslakImzaBekleyen { get; set; }

        public int SureciTamamlananlar { get; set; }

        public int ToplamIzin { get; set; }
    }

}