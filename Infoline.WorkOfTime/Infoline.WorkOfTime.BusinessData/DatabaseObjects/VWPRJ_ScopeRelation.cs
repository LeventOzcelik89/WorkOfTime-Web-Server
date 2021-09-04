using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ScopeRelation : InfolineTable
    {
        public Guid? corporateId { get; set;}
        public Guid? storageId { get; set;}
        public Guid? inventoryId { get; set;}
        public Guid? projectId { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string corporateId_Title { get; set;}
        public string storageId_Title { get; set;}
        public string projectId_Title { get; set;}
        public string inventoryId_Title { get; set;}
    }
}
