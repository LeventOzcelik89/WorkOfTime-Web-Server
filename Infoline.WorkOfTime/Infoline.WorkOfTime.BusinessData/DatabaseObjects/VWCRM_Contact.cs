using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_Contact : InfolineTable
    {
        public string searchField { get; set;}
        public Guid? PresentationId { get; set;}
        public DateTime? ContactStartDate { get; set;}
        public DateTime? ContactEndDate { get; set;}
        public string Description { get; set;}
        public int? ContactType { get; set;}
        public int? ContactStatus { get; set;}
        public Guid? PresentationStageId { get; set;}
        public Guid? customerId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string PresentationStageId_Title { get; set;}
        public string PresentationStageId_Color { get; set;}
        public string ContactType_Title { get; set;}
        public string ContactStatus_Title { get; set;}
        public int? TotalContactPerson { get; set;}
        public string ContactTime_Title { get; set;}
        public string customerId_Title { get; set;}
        public string ManagingUserIds { get; set;}
        public string Presentation_Title { get; set;}
        public string name { get; set;}
        public string PresentationAndCustomer_Titles { get; set;}
        public Guid? CustomerCompanyId { get; set;}
        public Guid? ChannelCompanyId { get; set;}
        public string CustomerCompanyId_Title { get; set;}
        public IGeometry  CustomerCompanyLocation { get; set;}
    }
}
