﻿using Infoline.WorkOfTime.BusinessData;
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

    }





}