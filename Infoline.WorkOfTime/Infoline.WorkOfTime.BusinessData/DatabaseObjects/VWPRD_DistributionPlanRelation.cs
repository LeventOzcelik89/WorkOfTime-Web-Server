using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_DistributionPlanRelation : InfolineTable
    {
        public Guid? transactionId { get; set;}
        public Guid? distributionPlanId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
