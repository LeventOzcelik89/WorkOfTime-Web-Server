using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskOperation : InfolineTable
    {
        public Guid? taskId { get; set;}
        public Guid? userId { get; set;}
        public int? status { get; set;}
        public string description { get; set;}
        public IGeometry  location { get; set;}
        public Guid? fixtureId { get; set;}
        public double? battery { get; set;}
        public short? subject { get; set;}
        public Guid? dataId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string subject_Title { get; set;}
        public string user_Title { get; set;}
        public string fixture_Title { get; set;}
        public string passingTime { get; set;}
        public string createdPhoto { get; set;}
        public double? distance { get; set;}
        public string task_Name { get; set;}
        public Guid? formResultId { get; set;}
        public Guid? formId { get; set;}
        public string jsonResult { get; set;}
    }
}
