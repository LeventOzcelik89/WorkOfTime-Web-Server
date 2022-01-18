using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_DistributionPlanRelation : InfolineTable
    {
        public Guid? transactionId { get; set;}
        public Guid? distributionPlanId { get; set;}
    }
}
