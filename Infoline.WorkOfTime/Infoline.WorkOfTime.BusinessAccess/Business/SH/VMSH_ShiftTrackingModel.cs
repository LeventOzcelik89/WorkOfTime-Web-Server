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

                        var tableTitle = "-";
                        var lastRecord = db.GetVWSH_ShiftTrackingFirstByUseridBeforeDateAndTypeInvetory(shiftTracking.userId.Value, new DateTime(startDate.Year, startDate.Month, startDate.Day, 23, 59, 59));
                        
                        if(lastRecord != null)
                        {
                            tableTitle = lastRecord.table_Title;
                        }
                        listData.Add(new VMSH_ShiftTrackingReport
                        {
                            totalWorking = workingHoursStringValue.ToString(),
                            CompanyId_Title = shiftTracking.CompanyId_Title,
                            UserId_Title = shiftTracking.UserId_Title,
                            startDate = startDate,
                            endDate = endDate,
                            date = startDate,
                            userId = shiftTracking.userId,
                            table_Title = tableTitle
                        });
                    }
                    startDate = startDate.AddDays(1);
                }

            }

            return listData.OrderBy(a => a.date).ToList();
        }

        public List<VMSH_ShiftTrackingReport> GetGeneralDataReportResult(DateTime startDate, DateTime endDate, List<Guid> userIds)
        {
            var db = new WorkOfTimeDatabase();
            var ourPersons = new List<VWSH_User>();
            if (userIds.Count() > 0)
            {
                ourPersons = db.GetVWSH_UserByIds(userIds.ToArray()).ToList();
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
                //while(ourPersons.Count > 0)
                //{
                listModel.AddRange(ourPersons.Select(x => new VMSH_ShiftTrackingReport
                {
                    userId = x.id,
                    startDate = startDate,
                    endDate = endDate,
                    //CompanyId_Title = x.Company_Title,
                    UserId_Title = x.FullName
                }));

                //ourPersons.RemoveAt(0);
                //}


                var workingTimes = TenantConfig.Tenant.Config.WorkingTimes;
                foreach (var shiftTracking in listModel.ToList())
                {
                    var userPermits = db.GetVWINV_PermitByIdUser(shiftTracking.userId.Value);
                    var officalPermits = db.GetVWINV_PermitOfficialByCalendar(startDate, endDate.AddMinutes(1));

                    var newPersonStartDate = startDate;
                    while (newPersonStartDate <= endDate)
                    {
                        var shiftStartTime = newPersonStartDate;
                        var shiftEndTime = newPersonStartDate.AddDays(1).AddMinutes(-1);


                        var day = shiftStartTime.DayOfWeek;
                        var dayWorkHour = workingTimes[day];
                        var morningStartTime = dayWorkHour.allowTimes[0].Start;
                        var morningEndTime = dayWorkHour.allowTimes[0].End;
                        var eveningStartTime = dayWorkHour.allowTimes[1].Start;
                        var eveningEndTime = dayWorkHour.allowTimes[1].End;

                        var todayShiftStartDate = newPersonStartDate.AddHours(morningStartTime.Hours).AddMinutes(morningStartTime.Minutes);
                        var todayShiftEndDate = newPersonStartDate.AddHours(eveningEndTime.Hours).AddMinutes(eveningEndTime.Minutes);

                        var todayPermitHour = "";
                        var todayPermitMinutes = 0.0;
                        var todayPermitType_Title = "";



                        var workingMinutes = GetCalculateDateDayShift(shiftTracking.userId.Value, newPersonStartDate, newPersonStartDate.AddDays(1).AddMilliseconds(-1),
                            shiftTrackings.Where(a => a.userId == shiftTracking.userId && a.timestamp >= newPersonStartDate && a.timestamp < newPersonStartDate.AddDays(1).AddMilliseconds(-1)).ToArray());

                        var shiftStatus = db.GetVWSH_ShiftTracking().Where(x => x.userId == shiftTracking.userId && x.timestamp >= newPersonStartDate && x.timestamp <= endDate).FirstOrDefault();


                        TimeSpan ts = TimeSpan.FromMinutes(workingMinutes);
                        var workingHoursStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                        var dailyShift = db.VWGetSH_ShiftTrackingByDateAndUserId(newPersonStartDate, shiftTracking.userId.Value).Reverse();

                        var shiftStart = dailyShift.FirstOrDefault();
                        var shiftEnd = dailyShift.LastOrDefault();

                        var lastStatus = shiftEnd == null ? "İşlem Yapılmadı" : shiftEnd.ShiftTrackingStatus_Title;


                        if (shiftStart != null)
                        {
                            shiftStartTime = shiftStart.shiftTrackingStatus != (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi ? newPersonStartDate : ((shiftStart != null) ? shiftStart.timestamp.Value : newPersonStartDate);
                        }


                        if (shiftEnd != null)
                        {
                            shiftEndTime = shiftEnd.shiftTrackingStatus != (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti ? newPersonStartDate.AddDays(1).AddMinutes(-1) : ((shiftEnd != null) ? shiftEnd.timestamp.Value : newPersonStartDate.AddDays(1).AddMinutes(-1));
                        }



                        //control permit 
                        var allDayPermit = false;
                        var abc = newPersonStartDate.AddDays(1).AddMilliseconds(-1);
                        var permits = userPermits.Where(x => /*x.ApproveStatus == (int)EnumINV_CompanyPersonAssessmentApproveStatus.SurecTamamlandi &&*/ x.StartDate.Value <= newPersonStartDate.AddDays(1).AddMilliseconds(-1) && x.EndDate.Value >= newPersonStartDate).ToArray();
                        var oPermits = officalPermits.Where(x => x.StartDate.Value <= newPersonStartDate.AddDays(1).AddMilliseconds(-1) && x.EndDate.Value >= newPersonStartDate).ToArray();
                        if (oPermits.Count() == 0)
                        {
                            foreach (var permit in permits)
                            {


                                // tüm gün izinli
                                if (permit.StartDate.Value <= todayShiftStartDate && permit.EndDate.Value >= todayShiftEndDate)
                                {
                                    listData.Add(new VMSH_ShiftTrackingReport
                                    {
                                        totalWorking = "Tam Gün İzinli",
                                        CompanyId_Title = shiftTracking.CompanyId_Title,
                                        UserId_Title = shiftTracking.UserId_Title,
                                        ShiftTrackingStatus_Title = permit.PermitType_Title + "<br>",
                                        startDate = new DateTime(shiftStartTime.Date.Year, shiftStartTime.Date.Month, shiftStartTime.Date.Day, shiftStartTime.Hour, shiftStartTime.Minute, shiftStartTime.Second),
                                        endDate = new DateTime(shiftEndTime.Date.Year, shiftEndTime.Date.Month, shiftEndTime.Date.Day, shiftEndTime.Hour, shiftEndTime.Minute, shiftEndTime.Second),
                                        date = new DateTime(newPersonStartDate.Year, newPersonStartDate.Month, newPersonStartDate.Day, 0, 0, 0),
                                        userId = shiftTracking.userId,
                                        totalBreak = "0 saat : 0 dakika",
                                        lateArrived = "0 saat : 0 dakika",
                                        earlyLeave = "0 saat : 0 dakika",
                                        extraShift = "Tam Gün İzinli"
                                    });
                                    allDayPermit = true;
                                    break;
                                }
                                else
                                {
                                    if (permit.StartDate.Value >= todayShiftStartDate && permit.StartDate.Value < todayShiftEndDate) //izinli geç gelinen süre
                                    {
                                        if (permit.EndDate.Value.Date == todayShiftStartDate.Date)
                                        {
                                            todayPermitType_Title += permit.PermitType_Title + " ";
                                            todayPermitMinutes = (permit.EndDate.Value - permit.StartDate.Value).TotalMinutes;
                                        }
                                        else
                                        {
                                            todayPermitType_Title += permit.PermitType_Title + " ";
                                            todayPermitMinutes = (shiftStartTime - permit.StartDate.Value).TotalMinutes;
                                        }

                                    }
                                    else if (permit.EndDate.Value > todayShiftStartDate && permit.EndDate.Value < todayShiftEndDate)
                                    {
                                        todayPermitType_Title += permit.PermitType_Title + " ";
                                        todayPermitMinutes = (permit.EndDate.Value - todayShiftStartDate).TotalMinutes;
                                    }
                                }

                            }
                        }
                        else
                        {
                            //tam gün izinli
                            if (((oPermits[0].StartDate.Value.Hour * 60 + oPermits[0].StartDate.Value.Minute) <= morningStartTime.TotalMinutes) && ((oPermits[0].EndDate.Value.Hour * 60 + oPermits[0].EndDate.Value.Minute) >= eveningEndTime.TotalMinutes))
                            {
                                listData.Add(new VMSH_ShiftTrackingReport
                                {
                                    totalWorking = "Tam Gün İzinli",
                                    CompanyId_Title = shiftTracking.CompanyId_Title,
                                    UserId_Title = shiftTracking.UserId_Title,
                                    ShiftTrackingStatus_Title = oPermits[0].Type_Title + "<br>",
                                    startDate = new DateTime(shiftStartTime.Date.Year, shiftStartTime.Date.Month, shiftStartTime.Date.Day, shiftStartTime.Hour, shiftStartTime.Minute, shiftStartTime.Second),
                                    endDate = new DateTime(shiftEndTime.Date.Year, shiftEndTime.Date.Month, shiftEndTime.Date.Day, shiftEndTime.Hour, shiftEndTime.Minute, shiftEndTime.Second),
                                    date = new DateTime(newPersonStartDate.Year, newPersonStartDate.Month, newPersonStartDate.Day, 0, 0, 0),
                                    userId = shiftTracking.userId,
                                    totalBreak = "0 saat : 0 dakika",
                                    lateArrived = "0 saat : 0 dakika",
                                    earlyLeave = "0 saat : 0 dakika",
                                    extraShift = "Tam Gün İzinli"
                                });
                                allDayPermit = true;
                            }
                            else
                            {
                                if (oPermits[0].StartDate.Value >= todayShiftStartDate && oPermits[0].StartDate.Value < todayShiftEndDate) //izinli geç gelinen süre
                                {
                                    if (oPermits[0].EndDate.Value.Date == todayShiftStartDate.Date)
                                    {
                                        todayPermitType_Title += oPermits[0].Type_Title + " ";
                                        todayPermitMinutes = (oPermits[0].EndDate.Value - oPermits[0].StartDate.Value).TotalMinutes;
                                    }
                                    else
                                    {
                                        todayPermitType_Title += oPermits[0].Type_Title + " ";
                                        todayPermitMinutes = (shiftStartTime - oPermits[0].StartDate.Value).TotalMinutes;
                                    }

                                }
                                else if (oPermits[0].EndDate.Value > todayShiftStartDate && oPermits[0].EndDate.Value < todayShiftEndDate)
                                {
                                    todayPermitType_Title += oPermits[0].Type_Title + " ";
                                    todayPermitMinutes = (oPermits[0].EndDate.Value - todayShiftStartDate).TotalMinutes;
                                }
                            }
                        }


                        if (allDayPermit)
                        {
                            newPersonStartDate = newPersonStartDate.AddDays(1);
                            continue;
                        }
                        else
                        {
                            TimeSpan timeSpan = TimeSpan.FromMinutes(todayPermitMinutes);
                            todayPermitHour = $"{(int)timeSpan.TotalHours} saat : {timeSpan.Minutes} dakika";
                        }


                        //workingMinutes -= todayPermitMinutes;


                        var breakMinutes = (shiftEndTime - shiftStartTime).TotalMinutes - workingMinutes - todayPermitMinutes;
                        ts = TimeSpan.FromMinutes(breakMinutes);
                        var breakHoursStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";


                        var lateArrived = (dayWorkHour.allowTimes[0].Start - new TimeSpan(shiftStartTime.Hour, shiftStartTime.Minute, shiftStartTime.Second)).TotalMinutes;
                        ts = TimeSpan.FromMinutes(lateArrived);

                        lateArrived = lateArrived + todayPermitMinutes;
                        var lateArrivedString = lateArrived >= 0 ? "YOK" : $"{(int)ts.TotalHours * -1} saat : {ts.Minutes * -1} dakika";

                        var earlyLeave = (dayWorkHour.allowTimes[1].End - new TimeSpan(shiftEndTime.Hour, shiftEndTime.Minute, shiftEndTime.Second)).TotalMinutes;
                        ts = TimeSpan.FromMinutes(earlyLeave);
                        var earlyLeaveString = earlyLeave <= 0 ? "YOK" : $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";


                        var extraShiftString = "Tatil Günü";
                        if (!(morningStartTime.TotalMinutes == 0 && morningEndTime.TotalMinutes == 0 && eveningStartTime.TotalMinutes == 0 && eveningEndTime.TotalMinutes == 0))
                        {
                            var dailyShiftMinutes = ((morningEndTime - morningStartTime) + (eveningEndTime - eveningStartTime)).TotalMinutes;
                            var extraShift = workingMinutes - dailyShiftMinutes + todayPermitMinutes;
                            if (extraShift == 0)
                            {
                                extraShiftString = "Tam Mesai Saatlerince Çalışmıştır";
                            }
                            else
                            {
                                ts = TimeSpan.FromMinutes(extraShift >= 0 ? extraShift : (extraShift * -1));
                                extraShiftString = ($"{(int)ts.TotalHours} saat : {ts.Minutes} dakika");
                                extraShiftString = extraShift < 0 ? (extraShiftString + " Az Çalışıldı") : (extraShiftString) + " Fazla Çalışıldı";
                            }
                        }
                        else
                        {
                            breakHoursStringValue = "-";
                            lateArrivedString = "Tatil Günü";
                            earlyLeaveString = "Tatil Günü";
                        }

                        listData.Add(new VMSH_ShiftTrackingReport
                        {
                            totalWorking = workingHoursStringValue.ToString(),
                            CompanyId_Title = shiftTracking.CompanyId_Title,
                            UserId_Title = shiftTracking.UserId_Title,
                            ShiftTrackingStatus_Title = todayPermitMinutes == 0.0 ? (lastStatus + "<br>") : (todayPermitType_Title + "<br>" + todayPermitHour),
                            startDate = new DateTime(shiftStartTime.Date.Year, shiftStartTime.Date.Month, shiftStartTime.Date.Day, shiftStartTime.Hour, shiftStartTime.Minute, shiftStartTime.Second),
                            endDate = new DateTime(shiftEndTime.Date.Year, shiftEndTime.Date.Month, shiftEndTime.Date.Day, shiftEndTime.Hour, shiftEndTime.Minute, shiftEndTime.Second),
                            date = new DateTime(newPersonStartDate.Year, newPersonStartDate.Month, newPersonStartDate.Day, 0, 0, 0),
                            userId = shiftTracking.userId,
                            totalBreak = breakHoursStringValue.ToString(),
                            lateArrived = lateArrivedString.ToString(),
                            earlyLeave = earlyLeaveString.ToString(),
                            extraShift = extraShiftString.ToString()
                        });
                        newPersonStartDate = newPersonStartDate.AddDays(1);
                    }

                }

            }

            //return listData.OrderBy(a => a.date).ToList();
            return listData.ToList();
        }

        public List<VMSH_ShiftTrackingReport> GetGeneralDataReportResultTotal(DateTime startDate, DateTime endDate, List<Guid> userIds)
        {
            var db = new WorkOfTimeDatabase();
            var ourPersons = new List<VWSH_User>();
            if (userIds.Count() > 0)
            {
                ourPersons = db.GetVWSH_UserByIds(userIds.ToArray()).ToList();
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
                //while(ourPersons.Count > 0)
                //{
                listModel.AddRange(ourPersons.Select(x => new VMSH_ShiftTrackingReport
                {
                    userId = x.id,
                    startDate = startDate,
                    endDate = endDate,
                    //CompanyId_Title = x.Company_Title,
                    UserId_Title = x.FullName
                }));

                //ourPersons.RemoveAt(0);
                //}


                var workingTimes = TenantConfig.Tenant.Config.WorkingTimes;
                foreach (var shiftTracking in listModel.ToList())
                {
                    var totalWorkingMinutes = 0.0;
                    var totalBreakMinutes = 0.0;
                    var totalEarlyLeaveMinutes = 0.0;
                    var totalLateArrivedMinutes = 0.0;
                    var totalExtraShiftMinutes = 0.0;
                    var totalPermitMinutes = 0.0;


                    var userPermits = db.GetVWINV_PermitByIdUser(shiftTracking.userId.Value);
                    var officalPermits = db.GetVWINV_PermitOfficialByCalendar(startDate, endDate.AddMinutes(1));

                    var newPersonStartDate = startDate;
                    while (newPersonStartDate <= endDate)
                    {
                        var shiftStartTime = newPersonStartDate;
                        var shiftEndTime = newPersonStartDate.AddDays(1).AddMinutes(-1);
                        var day = shiftStartTime.DayOfWeek;
                        var dayWorkHour = workingTimes[day];
                        var morningStartTime = dayWorkHour.allowTimes[0].Start;
                        var morningEndTime = dayWorkHour.allowTimes[0].End;
                        var eveningStartTime = dayWorkHour.allowTimes[1].Start;
                        var eveningEndTime = dayWorkHour.allowTimes[1].End;

                        var todayPermitMinutes = 0.0;

                        var workingMinutes = GetCalculateDateDayShift(shiftTracking.userId.Value, newPersonStartDate, newPersonStartDate.AddDays(1).AddMilliseconds(-1),
                            shiftTrackings.Where(a => a.userId == shiftTracking.userId && a.timestamp >= newPersonStartDate && a.timestamp < newPersonStartDate.AddDays(1).AddMilliseconds(-1)).ToArray());

                        var shiftStatus = db.GetVWSH_ShiftTracking().Where(x => x.userId == shiftTracking.userId && x.timestamp >= newPersonStartDate && x.timestamp <= endDate).FirstOrDefault();


                        var dailyShift = db.VWGetSH_ShiftTrackingByDateAndUserId(newPersonStartDate, shiftTracking.userId.Value).Reverse();

                        var shiftStart = dailyShift.FirstOrDefault();
                        var shiftEnd = dailyShift.LastOrDefault();

                        var lastStatus = shiftEnd == null ? "İşlem Yapılmadı" : shiftEnd.ShiftTrackingStatus_Title;


                        if (shiftStart != null)
                        {
                            shiftStartTime = shiftStart.shiftTrackingStatus != (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi ? newPersonStartDate : ((shiftStart != null) ? shiftStart.timestamp.Value : newPersonStartDate);
                        }


                        if (shiftEnd != null)
                        {
                            shiftEndTime = shiftEnd.shiftTrackingStatus != (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti ? newPersonStartDate.AddDays(1).AddMinutes(-1) : ((shiftEnd != null) ? shiftEnd.timestamp.Value : newPersonStartDate.AddDays(1).AddMinutes(-1));
                        }

                        var todayShiftStartDate = newPersonStartDate.AddHours(morningStartTime.Hours).AddMinutes(morningStartTime.Minutes);
                        var todayShiftEndDate = newPersonStartDate.AddHours(eveningEndTime.Hours).AddMinutes(eveningEndTime.Minutes);

                        //control permit 
                        var allDayPermit = false;

                        var permits = userPermits.Where(x => /*x.ApproveStatus == (int)EnumINV_CompanyPersonAssessmentApproveStatus.SurecTamamlandi &&*/ x.StartDate.Value <= newPersonStartDate.AddDays(1).AddMilliseconds(-1) && x.EndDate.Value >= newPersonStartDate).ToArray();
                        var oPermits = officalPermits.Where(x => x.StartDate.Value <= newPersonStartDate.AddDays(1).AddMilliseconds(-1) && x.EndDate.Value >= newPersonStartDate).ToArray();
                        if (oPermits.Count() == 0)
                        {
                            foreach (var permit in permits)
                            {

                                // tüm gün izinli
                                if (permit.StartDate.Value <= todayShiftStartDate && permit.EndDate.Value >= todayShiftEndDate)
                                {
                                    todayPermitMinutes += (todayShiftEndDate - todayShiftStartDate).TotalMinutes;
                                    allDayPermit = true;
                                    break;
                                }
                                else
                                {
                                    if (permit.StartDate.Value >= todayShiftStartDate && permit.StartDate.Value < todayShiftEndDate) //izinli geç gelinen süre
                                    {
                                        if (permit.EndDate.Value.Date == todayShiftStartDate.Date)
                                        {
                                            todayPermitMinutes = (permit.EndDate.Value - permit.StartDate.Value).TotalMinutes;
                                        }
                                        else
                                        {
                                            todayPermitMinutes = (shiftStartTime - permit.StartDate.Value).TotalMinutes;
                                        }

                                    }
                                    else if (permit.EndDate.Value > todayShiftStartDate && permit.EndDate.Value < todayShiftEndDate)
                                    {
                                        todayPermitMinutes = (permit.EndDate.Value - todayShiftStartDate).TotalMinutes;
                                    }
                                }

                            }
                        }
                        else
                        {

                            //tam gün izinli
                            if (((oPermits[0].StartDate.Value.Hour * 60 + oPermits[0].StartDate.Value.Minute) <= morningStartTime.TotalMinutes) && ((oPermits[0].EndDate.Value.Hour * 60 + oPermits[0].EndDate.Value.Minute) >= eveningEndTime.TotalMinutes))
                            {
                                todayPermitMinutes += (todayShiftEndDate - todayShiftStartDate).TotalMinutes;
                                allDayPermit = true;
                            }
                            else
                            {
                                if (oPermits[0].StartDate.Value >= todayShiftStartDate && oPermits[0].StartDate.Value < todayShiftEndDate) //izinli geç gelinen süre
                                {
                                    if (oPermits[0].EndDate.Value.Date == todayShiftStartDate.Date)
                                    {
                                        todayPermitMinutes = (oPermits[0].EndDate.Value - oPermits[0].StartDate.Value).TotalMinutes;
                                    }
                                    else
                                    {
                                        todayPermitMinutes = (shiftStartTime - oPermits[0].StartDate.Value).TotalMinutes;
                                    }

                                }
                                else if (oPermits[0].EndDate.Value > todayShiftStartDate && oPermits[0].EndDate.Value < todayShiftEndDate)
                                {
                                    todayPermitMinutes = (oPermits[0].EndDate.Value - todayShiftStartDate).TotalMinutes;
                                }
                            }

                        }

                        if (allDayPermit)
                        {
                            newPersonStartDate = newPersonStartDate.AddDays(1);
                            totalPermitMinutes += totalPermitMinutes;
                            continue;
                        }

                        var breakMinutes = 0.0;
                        //tatil günü değilse mola ekle
                        if (!(morningStartTime.TotalMinutes == 0 && morningEndTime.TotalMinutes == 0 && eveningStartTime.TotalMinutes == 0 && eveningEndTime.TotalMinutes == 0))
                        {
                            breakMinutes = (shiftEndTime - shiftStartTime).TotalMinutes - workingMinutes - todayPermitMinutes;
                        }


                        var lateArrived = (new TimeSpan(shiftStartTime.Hour, shiftStartTime.Minute, shiftStartTime.Second) - dayWorkHour.allowTimes[0].Start).TotalMinutes;
                        lateArrived = lateArrived > 0 ? lateArrived : 0.0;

                        var earlyLeave = (dayWorkHour.allowTimes[1].End - new TimeSpan(shiftEndTime.Hour, shiftEndTime.Minute, shiftEndTime.Second)).TotalMinutes;
                        earlyLeave = earlyLeave > 0 ? earlyLeave : 0.0;

                        var extraShift = 0.0;

                        if (!(morningStartTime.TotalMinutes == 0 && morningEndTime.TotalMinutes == 0 && eveningStartTime.TotalMinutes == 0 && eveningEndTime.TotalMinutes == 0))
                        {
                            var dailyShiftMinutes = ((morningEndTime - morningStartTime) + (eveningEndTime - eveningStartTime)).TotalMinutes;
                            extraShift = workingMinutes - dailyShiftMinutes + todayPermitMinutes;
                        }

                        totalWorkingMinutes += workingMinutes;
                        totalBreakMinutes += breakMinutes;
                        totalPermitMinutes += todayPermitMinutes;
                        totalEarlyLeaveMinutes += earlyLeave;
                        totalExtraShiftMinutes += extraShift;
                        totalLateArrivedMinutes += lateArrived;

                        newPersonStartDate = newPersonStartDate.AddDays(1);
                    }

                    TimeSpan ts = TimeSpan.FromMinutes(totalWorkingMinutes);
                    var totalWorkingMinutesStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                    ts = TimeSpan.FromMinutes(totalBreakMinutes);
                    var totalBreakMinutesStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                    ts = TimeSpan.FromMinutes(totalPermitMinutes);
                    var totalPermitMinutesStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika";

                    ts = TimeSpan.FromMinutes(totalEarlyLeaveMinutes);
                    var totalEarlyLeaveMinutesStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika erken çıkmıştır.";

                    ts = TimeSpan.FromMinutes(totalExtraShiftMinutes);
                    var totalExtraShiftMinutesStringValue = ts.TotalSeconds > 0 ? ($"{(int)ts.TotalHours} saat : {ts.Minutes} dakika fazla çalışmıştır.") : ($"{(int)ts.TotalHours * -1} saat : {ts.Minutes * -1} dakika az çalışmıştır.");

                    ts = TimeSpan.FromMinutes(totalLateArrivedMinutes);
                    var totalLateArrivedMinutesStringValue = $"{(int)ts.TotalHours} saat : {ts.Minutes} dakika geç gelmiştir.";


                    listData.Add(new VMSH_ShiftTrackingReport
                    {
                        CompanyId_Title = shiftTracking.CompanyId_Title,
                        UserId_Title = shiftTracking.UserId_Title,
                        startDate = startDate,
                        endDate = endDate,
                        userId = shiftTracking.userId,
                        totalWorking = totalWorkingMinutesStringValue.ToString(),
                        totalBreak = totalBreakMinutesStringValue.ToString(),
                        lateArrived = totalLateArrivedMinutesStringValue.ToString(),
                        earlyLeave = totalEarlyLeaveMinutesStringValue.ToString(),
                        extraShift = totalExtraShiftMinutesStringValue.ToString(),
                        permitTime = totalPermitMinutesStringValue.ToString()
                    });
                }

            }

            //return listData.OrderBy(a => a.UserId_Title).ToList();
            return listData.ToList();
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
        public string permitTime { get; set; }

    }





}
