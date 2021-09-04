using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPDS_FormPermitTask : InfolineTable
    {
        public string taskName { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public bool? status { get; set;}
        public Guid? evaluateFormId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string formName_title { get; set;}
    }
}
