using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_Contact : InfolineTable
    {
        public Guid? PresentationId { get; set;}
        public DateTime? ContactStartDate { get; set;}
        public DateTime? ContactEndDate { get; set;}
        public string Description { get; set;}
        public int? ContactType { get; set;}
        public int? ContactStatus { get; set;}
        public Guid? PresentationStageId { get; set;}
        public Guid? customerId { get; set;}
        public Guid? storageId { get; set;}
    }
}
