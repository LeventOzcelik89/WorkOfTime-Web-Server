using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_DistributionPlan : InfolineTable
    {
        public DateTime? date { get; set;}
        public string description { get; set;}
        public string code { get; set;}
        public short? status { get; set;}
        public Guid? outputId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string outputId_Title { get; set;}
        public string outputCompanyId_Title { get; set;}
    }
}
