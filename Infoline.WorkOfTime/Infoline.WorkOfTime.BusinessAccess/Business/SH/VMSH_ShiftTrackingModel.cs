using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMShiftTrackingModel
    {
        public List<VMSH_ShiftTrackingReport> GetDataReportResult(DateTime date, Guid? userId)
        {
            var db = new WorkOfTimeDatabase();
            var ourPersons = new List<VWSH_User>();
            if (userId.HasValue)
            {
                ourPersons.Add(db.GetVWSH_UserById(userId.Value));
            }
            else
            {
                ourPersons = db.GetVWSH_UserMyPerson().Where(x => x.IsWorking == true).ToList();
            }

            var shiftTrackings = db.VWGetSH_ShiftTrackingByDate(date);
            var listModel = new List<VMSH_ShiftTrackingReport>();
            var listData = new List<VMSH_ShiftTrackingReport>();



            listModel.AddRange(ourPersons.Select(x => new VMSH_ShiftTrackingReport
            {
                userId = x.id,
                date = date,
                shiftTrackingStart = shiftTrackings.Where(t => t.shiftTrackingStatus == (Int32)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi && t.userId == x.id).OrderBy(c => c.timestamp).FirstOrDefault() ?? new VMSH_ShiftTrackingReport { timestamp = DateTime.Now },
                shiftTrackingEnd = shiftTrackings.Where(t => t.shiftTrackingStatus == (Int32)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti && t.userId == x.id).OrderByDescending(c => c.timestamp).FirstOrDefault() ?? new VMSH_ShiftTrackingReport { timestamp = DateTime.Now },
                CompanyId_Title = x.Company_Title,
                UserId_Title = x.FullName
            }));

            foreach (var shiftTracking in listModel.ToList())
            {
                var workingMinutes = GetCalculateDayShift(shiftTracking.userId.Value, date, shiftTrackings.Where(a => a.userId == shiftTracking.userId).ToArray());

                TimeSpan ts = TimeSpan.FromMinutes(workingMinutes);
                var workingHoursStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                var userPhoto = db.GetSysFilesFilePathByDataTableAndFileGroupAndDataId("SH_User", "Profil Resmi", shiftTracking.userId.Value);

                listData.Add(new VMSH_ShiftTrackingReport
                {
                    totalWorking = workingHoursStringValue.ToString(),
                    CompanyId_Title = shiftTracking.CompanyId_Title,
                    UserId_Title = shiftTracking.UserId_Title,
                    date = shiftTracking.date,
                    userId = shiftTracking.userId,
                    userPhoto = userPhoto?.FilePath
                });
            }

            return listData;
        }

        public double GetCalculateDayShift(Guid userId, DateTime dateTime, VWSH_ShiftTracking[] shiftTrackingReport)
        {
            if (shiftTrackingReport == null || shiftTrackingReport.Count() < 1)
            {
                var db = new WorkOfTimeDatabase();
                var lastAction = db.GetSH_ShiftTrackingFirstByUseridBeforeDate(userId, dateTime);
                if (lastAction != null)
                {
                    if (lastAction.shiftTrackingStatus != (int)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti && lastAction.shiftTrackingStatus == (int)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi)
                    {
                        if (lastAction.shiftTrackingStatus != (int)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi)
                        {
                            var betweenTime = (int)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti - (int)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi;
                            return (60 * 24) - betweenTime;
                        }
                        return 60 * 24;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            }
            else
            {
                var shiftMinutesList = new List<double>();
                var shiftTrackingReportList = shiftTrackingReport.OrderBy(a => a.timestamp).ToList();

                while (shiftTrackingReportList.Count() > 0)
                {
                    DateTime firstStartDate;
                    DateTime firstFinishDate;

                    var firstValue = shiftTrackingReportList.FirstOrDefault().shiftTrackingStatus;
                    if (firstValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi || firstValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti)
                    {
                        firstStartDate = shiftTrackingReportList.FirstOrDefault().timestamp.Value;
                        shiftTrackingReportList.Remove(shiftTrackingReportList.FirstOrDefault());

                        foreach (var item in shiftTrackingReportList.ToList())
                        {
                            if (item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi || item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti)
                                shiftTrackingReportList.Remove(item);
                            else
                                break;
                        }
                    }
                    else
                    {
                        firstStartDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 00, 00, 000);
                    }


                    if (shiftTrackingReportList.Count() > 0)
                    {
                        var firstFinishValue = shiftTrackingReportList.FirstOrDefault().shiftTrackingStatus;
                        if (firstFinishValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti || firstFinishValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi)
                        {
                            firstFinishDate = shiftTrackingReportList.FirstOrDefault().timestamp.Value;
                            shiftTrackingReportList.Remove(shiftTrackingReportList.FirstOrDefault());

                            foreach (var item in shiftTrackingReportList.ToList())
                            {
                                if (item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti || item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi)
                                    shiftTrackingReportList.Remove(item);
                                else
                                    break;
                            }
                        }
                        else
                        {
                            if (dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month && dateTime.Day == DateTime.Now.Day)
                                firstFinishDate = DateTime.Now;
                            else
                                firstFinishDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                        }
                    }
                    else
                    {
                        if (dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month && dateTime.Day == DateTime.Now.Day)
                            firstFinishDate = DateTime.Now;
                        else
                            firstFinishDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                    }

                    shiftMinutesList.Add((firstFinishDate - firstStartDate).TotalMinutes);
                }
                var totalMinutes = shiftMinutesList.Where(a => a > 0).Sum();
                return totalMinutes;
            }
        }
        public double GetCalculateDateDayShift(Guid userId, DateTime shiftstart, DateTime shiftend, VWSH_ShiftTracking[] shiftTrackingReport)
        {
            if (shiftTrackingReport == null || shiftTrackingReport.Count() < 1)
            {


                var db = new WorkOfTimeDatabase();
                var lastAction = db.GetSH_ShiftTrackingFirstByUseridBeforeDate(userId, shiftstart);
                if (lastAction != null)
                {
                    if (lastAction.shiftTrackingStatus != (int)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti && lastAction.shiftTrackingStatus != (int)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti || lastAction.shiftTrackingStatus == (int)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi)
                    {
                        return 60 * 24;
                    }
                    else
                    {
                        return 0;
                    }
                }

                return 0;
            }
            else
            {
                var shiftMinutesList = new List<double>();
                var shiftTrackingReportList = shiftTrackingReport.OrderBy(a => a.timestamp).ToList();

                while (shiftTrackingReportList.Count() > 0)
                {
                    DateTime firstStartDate;
                    DateTime firstFinishDate;

                    var firstValue = shiftTrackingReportList.FirstOrDefault().shiftTrackingStatus;
                    if (firstValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi || firstValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti)
                    { 
                        firstStartDate = shiftTrackingReportList.FirstOrDefault().timestamp.Value;
                        shiftTrackingReportList.Remove(shiftTrackingReportList.FirstOrDefault());

                        foreach (var item in shiftTrackingReportList.ToList())
                        {
                            if (item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi || item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti)
                                shiftTrackingReportList.Remove(item);
                            else
                                break;
                        }
                    }
                    else
                    {
                        firstStartDate = new DateTime(shiftstart.Year, shiftstart.Month, shiftstart.Day, 00, 00, 000);
                    }


                    if (shiftTrackingReportList.Count() > 0)
                    {
                        var firstFinishValue = shiftTrackingReportList.FirstOrDefault().shiftTrackingStatus;
                        if (firstFinishValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti || firstFinishValue == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi)
                        {
                            firstFinishDate = shiftTrackingReportList.FirstOrDefault().timestamp.Value;
                            shiftTrackingReportList.Remove(shiftTrackingReportList.FirstOrDefault());

                            foreach (var item in shiftTrackingReportList.ToList())
                            {
                                if (item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti || item.shiftTrackingStatus == (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi)
                                    shiftTrackingReportList.Remove(item);
                                else
                                    break;
                            }
                        }
                        else
                        {
                            if (shiftstart.Year == DateTime.Now.Year && shiftstart.Month == DateTime.Now.Month && shiftstart.Day == DateTime.Now.Day)
                                firstFinishDate = DateTime.Now;
                            else
                                firstFinishDate = new DateTime(shiftstart.Year, shiftstart.Month, shiftstart.Day, 23, 59, 59);
                        }
                    }
                    else
                    {
                        if (shiftstart.Year == DateTime.Now.Year && shiftstart.Month == DateTime.Now.Month && shiftstart.Day == DateTime.Now.Day)
                            firstFinishDate = DateTime.Now;
                        else
                            firstFinishDate = new DateTime(shiftstart.Year, shiftstart.Month, shiftstart.Day, 23, 59, 59);
                    }

                    shiftMinutesList.Add((firstFinishDate - firstStartDate).TotalMinutes);
                }
                var totalMinutes = shiftMinutesList.Where(a => a > 0).Sum();
                return totalMinutes;
            }
        }

        public List<VMSH_ShiftTrackingReport> GetDateDataReportResult(DateTime startDate, DateTime endDate, Guid? userId)

        {
            var db = new WorkOfTimeDatabase();
            var ourPersons = new List<VWSH_User>();
            if (userId.HasValue)
            {
                ourPersons.Add(db.GetVWSH_UserById(userId.Value));
            }
            else
            {
                ourPersons = db.GetVWSH_UserMyPerson().ToList();
            }

            var shiftTrackings = db.VWGetSH_ShiftTrackingByStartAndEndDate(startDate, endDate);
            var listModel = new List<VMSH_ShiftTrackingReport>();
            var listData = new List<VMSH_ShiftTrackingReport>();

            if (shiftTrackings.Count() > 0)
            {

                listModel.AddRange(ourPersons.Select(x => new VMSH_ShiftTrackingReport
                {
                    userId = ourPersons.FirstOrDefault()?.id,
                    startDate = startDate,
                    endDate = endDate,
                    CompanyId_Title = x.Company_Title,
                    UserId_Title = ourPersons.FirstOrDefault()?.FullName
                }));

                while (startDate <= endDate)
                {
                    foreach (var shiftTracking in listModel.ToList())
                    {
                        var workingMinutes = GetCalculateDateDayShift(shiftTracking.userId.Value, startDate, startDate.AddDays(1).AddMilliseconds(-1),
                            shiftTrackings.Where(a => a.userId == shiftTracking.userId && a.timestamp >= startDate && a.timestamp < startDate.AddDays(1).AddMilliseconds(-1)).ToArray());

                        TimeSpan ts = TimeSpan.FromMinutes(workingMinutes);
                        var workingHoursStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                        listData.Add(new VMSH_ShiftTrackingReport
                        {
                            totalWorking = workingHoursStringValue.ToString(),
                            CompanyId_Title = shiftTracking.CompanyId_Title,
                            UserId_Title = shiftTracking.UserId_Title,
                            startDate = startDate,
                            endDate = endDate,
                            date = startDate,
                            userId = shiftTracking.userId
                        });
                    }
                    startDate = startDate.AddDays(1);
                }

            }

            return listData.OrderBy(a => a.date).ToList();
        }

        public List<VMSH_ShiftTrackingReport> GetGeneralDataReportResult(DateTime startDate, DateTime endDate, Guid? userId)
        {
            var db = new WorkOfTimeDatabase();
            var ourPersons = new List<VWSH_User>();
            if (userId.HasValue)
            {
                ourPersons.Add(db.GetVWSH_UserById(userId.Value));
            }
            else
            {
                ourPersons = db.GetVWSH_UserMyPerson().ToList();
            }

            var shiftTrackings = db.VWGetSH_ShiftTrackingByStartAndEndDate(startDate, endDate);
            var listModel = new List<VMSH_ShiftTrackingReport>();
            var listData = new List<VMSH_ShiftTrackingReport>();

            if (shiftTrackings.Count() > 0)
            {

                listModel.AddRange(ourPersons.Select(x => new VMSH_ShiftTrackingReport
                {
                    userId = ourPersons.FirstOrDefault()?.id,
                    startDate = startDate,
                    endDate = endDate,
                    //CompanyId_Title = x.Company_Title,
                    UserId_Title = ourPersons.FirstOrDefault()?.FullName
                }));

                var workingTimes = TenantConfig.Tenant.Config.WorkingTimes;
                while (startDate <= endDate)
                {
                    foreach (var shiftTracking in listModel.ToList())
                    {

                        var workingMinutes = GetCalculateDateDayShift(shiftTracking.userId.Value, startDate, startDate.AddDays(1).AddMilliseconds(-1),
                            shiftTrackings.Where(a => a.userId == shiftTracking.userId && a.timestamp >= startDate && a.timestamp < startDate.AddDays(1).AddMilliseconds(-1)).ToArray());
                        var shiftStatus = db.GetVWSH_ShiftTracking().Where(x => x.userId == shiftTracking.userId && x.timestamp >= startDate && x.timestamp <= endDate).FirstOrDefault();


                        TimeSpan ts = TimeSpan.FromMinutes(workingMinutes);
                        var workingHoursStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                        var dailyShift = db.VWGetSH_ShiftTrackingByDateAndUserId(startDate, shiftTracking.userId.Value).Reverse();

                        var shiftStart = dailyShift.FirstOrDefault();
                        var shiftEnd = dailyShift.LastOrDefault();
                        var shiftStartTime = startDate;
                        var shiftEndTime = startDate.AddDays(1).AddMinutes(-1);


                        if (shiftStart != null)
                        {
                            shiftStartTime = shiftStart.shiftTrackingStatus != (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi ? startDate : ((shiftStart != null) ? shiftStart.timestamp.Value : startDate);
                        }


                        if (shiftEnd != null)
                        {
                            shiftEndTime = shiftEnd.shiftTrackingStatus != (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti ? startDate.AddDays(1).AddMinutes(-1) : ((shiftEnd != null) ? shiftEnd.timestamp.Value : startDate.AddDays(1).AddMinutes(-1));
                        }

                        var breakMinutes = (shiftEndTime - shiftStartTime).TotalMinutes - workingMinutes;
                        ts = TimeSpan.FromMinutes(breakMinutes);
                        var breakHoursStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                        var day = shiftStartTime.DayOfWeek;
                        var dayWorkHour = workingTimes[day];

                        var lateArrived = (dayWorkHour.allowTimes[0].Start - new TimeSpan(shiftStartTime.Hour, shiftStartTime.Minute, shiftStartTime.Second)).TotalMinutes;
                        ts = TimeSpan.FromMinutes(lateArrived);
                        var lateArrivedString = lateArrived >= 0 ? "YOK" : $"{(int)ts.TotalHours*-1} saat : {ts.Minutes*-1} dakika";

                        var earlyLeave = (dayWorkHour.allowTimes[1].End - new TimeSpan(shiftEndTime.Hour, shiftEndTime.Minute, shiftEndTime.Second)).TotalMinutes;
                        ts = TimeSpan.FromMinutes(earlyLeave);
                        var earlyLeaveString = earlyLeave <= 0 ? "YOK" : $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                        var morningStartTime = dayWorkHour.allowTimes[0].Start;
                        var morningEndTime = dayWorkHour.allowTimes[0].End;
                        var eveningStartTime = dayWorkHour.allowTimes[1].Start;
                        var eveningEndTime = dayWorkHour.allowTimes[1].End;

                        var extraShiftString = "Tatil Günü";
                        if (!(morningStartTime.TotalMinutes == 0 && morningEndTime.TotalMinutes == 0 && eveningStartTime.TotalMinutes == 0 && eveningEndTime.TotalMinutes == 0))
                        {
                            var dailyShiftMinutes = ((morningEndTime - morningStartTime) + (eveningEndTime - eveningStartTime)).TotalMinutes;
                            var extraShift = workingMinutes - dailyShiftMinutes;
                            ts = TimeSpan.FromMinutes(extraShift >= 0 ? extraShift : (extraShift * -1));
                            extraShiftString = ($"{(int)ts.TotalHours} saat : {ts.Minutes} dakika");
                            extraShiftString = extraShift < 0 ? (extraShiftString + " Az Çalışıldı") : (extraShiftString) + " Fazla Çalışıldı";
                        }
                        else
                        {
                            lateArrivedString = "Tatil Günü";
                            earlyLeaveString = "Tatil Günü";
                        }

                        listData.Add(new VMSH_ShiftTrackingReport
                        {
                            totalWorking = workingHoursStringValue.ToString(),
                            CompanyId_Title = shiftTracking.CompanyId_Title,    
                            UserId_Title = shiftTracking.UserId_Title,
                            ShiftTrackingStatus_Title = shiftStatus?.ShiftTrackingStatus_Title,
                            startDate = new DateTime(shiftStartTime.Date.Year, shiftStartTime.Date.Month, shiftStartTime.Date.Day, shiftStartTime.Hour, shiftStartTime.Minute, shiftStartTime.Second),
                            endDate = new DateTime(shiftEndTime.Date.Year, shiftEndTime.Date.Month, shiftEndTime.Date.Day, shiftEndTime.Hour, shiftEndTime.Minute, shiftEndTime.Second),
                            date = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0),
                            userId = shiftTracking.userId,
                            totalBreak = breakHoursStringValue.ToString(),
                            lateArrived = lateArrivedString.ToString(),
                            earlyLeave = earlyLeaveString.ToString(),
                            extraShift = extraShiftString.ToString()
                        });
                    }
                    startDate = startDate.AddDays(1);
                }

            }

            return listData.OrderBy(a => a.date).ToList();
        }

      
    }

    public class VMSH_ShiftTrackingModel : VWSH_ShiftTracking
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
    }
    public class VMSH_ShiftTrackingReport : VWSH_ShiftTracking
    {
        public DateTime date { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string totalWorking { get; set; }
        public VWSH_ShiftTracking shiftTrackingStart { get; set; }
        public VWSH_ShiftTracking shiftTrackingEnd { get; set; }
        public string userPhoto { get; set; }
        public string totalBreak { get; set; }
        public string earlyLeave { get; set; }
        public string lateArrived { get; set; }
        public string extraShift { get; set; }

    }





}
