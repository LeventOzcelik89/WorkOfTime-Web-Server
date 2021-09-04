using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRJ_ScopeRelation : InfolineTable
    {
        public Guid? projectId { get; set;}
        public Guid? corporateId { get; set;}
        public Guid? storageId { get; set;}
        public Guid? inventoryId { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
    }
}
