using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_DistributionPlan : InfolineTable
    {
        public DateTime? date { get; set;}
        public string description { get; set;}
        public string code { get; set;}
        public short? status { get; set;}
        public Guid? outputId { get; set;}
    }
}
