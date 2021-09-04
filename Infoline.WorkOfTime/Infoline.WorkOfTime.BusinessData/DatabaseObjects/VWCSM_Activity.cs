using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCSM_Activity : InfolineTable
    {
        public short? type { get; set;}
        public Guid? studentId { get; set;}
        public DateTime? date { get; set;}
        public int? duration { get; set;}
        public Guid? stageId { get; set;}
        public DateTime? contactDate { get; set;}
        public string description { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
        public string studentId_Title { get; set;}
        public string stageId_Title { get; set;}
        public string stageId_Color { get; set;}
    }
}
