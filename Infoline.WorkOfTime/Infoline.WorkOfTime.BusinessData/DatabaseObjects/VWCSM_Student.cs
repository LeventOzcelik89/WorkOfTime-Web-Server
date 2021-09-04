using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCSM_Student : InfolineTable
    {
        public string name { get; set;}
        public string phone { get; set;}
        public string email { get; set;}
        public bool? isAllowContact { get; set;}
        public Guid? schoolId { get; set;}
        public short? grade { get; set;}
        public short? source { get; set;}
        public Guid? locationId { get; set;}
        public string sourceDescription { get; set;}
        public string deparmentCurrent { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string isAllowContact_Title { get; set;}
        public string source_Title { get; set;}
        public string grade_Title { get; set;}
        public string schoolId_Title { get; set;}
        public string locationId_Title { get; set;}
        public int? activityCount { get; set;}
        public Guid? lastActivity_Id { get; set;}
        public DateTime? lastActivity_Date { get; set;}
        public string lastActivity_Description { get; set;}
        public Guid? lastActivity_StageId { get; set;}
        public Guid? lastActivity_userId { get; set;}
        public string lastActivity_userIdTitle { get; set;}
        public string department_Titles { get; set;}
        public string LastStageId_Color { get; set;}
        public string LastStageId_Title { get; set;}
        public string searchField { get; set;}
    }
}
