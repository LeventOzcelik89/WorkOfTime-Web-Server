using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskPlan : InfolineTable
    {
        public bool? enabled { get; set;}
        public string name { get; set;}
        public DateTime? frequencyStartDate { get; set;}
        public DateTime? frequencyEndDate { get; set;}
        public int? frequency { get; set;}
        public int? frequencyInterval { get; set;}
        public int? taskCreationTime { get; set;}
        public string times { get; set;}
        public string weekDays { get; set;}
        public string monthDays { get; set;}
        public Guid? templateId { get; set;}
        public int? monthFrequency { get; set;}
        public int? dayFrequency { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public int? task_Count { get; set;}
        public string frequency_Title { get; set;}
        public string taskCreationTime_Title { get; set;}
        public string templateId_Title { get; set;}
        public Guid? customerId { get; set;}
        public short? taskType { get; set;}
        public string helperUserIds { get; set;}
        public string assignableUserIds { get; set;}
        public string customerId_Title { get; set;}
        public Guid? customerStorageId { get; set;}
        public int? estimatedTaskMinute { get; set;}
    }
}
