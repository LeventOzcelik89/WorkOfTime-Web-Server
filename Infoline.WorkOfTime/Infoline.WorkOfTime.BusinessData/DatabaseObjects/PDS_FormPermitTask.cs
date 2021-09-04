using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PDS_FormPermitTask : InfolineTable
    {
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public bool? status { get; set;}
        public Guid? evaluateFormId { get; set;}
    }
}
