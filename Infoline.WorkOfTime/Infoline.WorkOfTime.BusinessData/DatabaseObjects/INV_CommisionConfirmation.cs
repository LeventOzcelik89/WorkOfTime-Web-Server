using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CommisionConfirmation : InfolineTable
    {
        public Guid? commissionId { get; set;}
        public Guid? userId { get; set;}
        public string description { get; set;}
        public Guid? ruleStageId { get; set;}
        public Guid? ruleId { get; set;}
    }
}
