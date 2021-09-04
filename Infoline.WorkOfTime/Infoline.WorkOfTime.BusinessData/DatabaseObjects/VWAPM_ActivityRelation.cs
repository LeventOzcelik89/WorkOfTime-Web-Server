using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWAPM_ActivityRelation : InfolineTable
    {
        public string name { get; set;}
        public short? type { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public string description { get; set;}
        public short? generalType { get; set;}
        public IGeometry  location { get; set;}
        public short? communicationType { get; set;}
        public short? notification { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string generalType_Title { get; set;}
        public string type_Title { get; set;}
        public string communicationType_Title { get; set;}
        public string notification_Title { get; set;}
        public string activityUserIds { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public string dataId_Title { get; set;}
    }
}
