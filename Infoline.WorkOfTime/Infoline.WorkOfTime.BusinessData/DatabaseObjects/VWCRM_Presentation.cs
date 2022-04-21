using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_Presentation : InfolineTable
    {
        public string searchField { get; set;}
        public string Name { get; set;}
        public Guid? SalesPersonId { get; set;}
        public Guid? ChannelCompanyId { get; set;}
        public Guid? CustomerCompanyId { get; set;}
        public Guid? PresentationStageId { get; set;}
        public DateTime? CommitmentEndDate { get; set;}
        public double? OpponentInvoiceAmount { get; set;}
        public double? VodafoneOffer { get; set;}
        public double? CompletionRate { get; set;}
        public DateTime? EstimatedCompletionDate { get; set;}
        public long? Budget { get; set;}
        public int? PriorityLevel { get; set;}
        public bool? hasContact { get; set;}
        public short? PlaceofArrival { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string SalesPerson_Title { get; set;}
        public string ChannelCompany_Title { get; set;}
        public string CustomerCompany_Title { get; set;}
        public string ManagingUserIds { get; set;}
        public string cityTitle { get; set;}
        public string townTitle { get; set;}
        public string CustomerCompany_Phone { get; set;}
        public IGeometry  CustomerCompanyLocation { get; set;}
        public int? DaysSinceVisit { get; set;}
        public double? TotalPoints { get; set;}
        public string Stage_Color { get; set;}
        public string Stage_Title { get; set;}
        public DateTime? PresentationStageChangedDate { get; set;}
        public int? TotalContact { get; set;}
        public string LastDescription { get; set;}
        public string LastStatus { get; set;}
        public string PlaceofArrival_Title { get; set;}
        public DateTime? Last_ContactDate { get; set;}
        public DateTime? LastActivityDate { get; set;}
        public string LastActivityCreatedBy_Title { get; set;}
        public string Product_Titles { get; set;}
        public double? lastTenderTotalAmount { get; set;}
        public string createdByPhoto { get; set;}
        public string salesPersonPhoto { get; set;}
    }
}
