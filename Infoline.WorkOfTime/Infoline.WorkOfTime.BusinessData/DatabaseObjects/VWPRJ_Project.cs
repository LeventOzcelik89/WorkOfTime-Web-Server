using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_Project : InfolineTable
    {
        public string ProjectName { get; set;}
        public string ProjectCode { get; set;}
        public string ProjectScope { get; set;}
        public DateTime? ProjectStartDate { get; set;}
        public DateTime? ProjectEndDate { get; set;}
        public DateTime? ProjectConfirmDate { get; set;}
        public bool? IsConfirm { get; set;}
        public bool? IsActive { get; set;}
        public int? ProjectType { get; set;}
        public Guid? CompanyId { get; set;}
        public Guid? CorporationId { get; set;}
        public string TenderNo { get; set;}
        public string TenderName { get; set;}
        public string TenderWrite { get; set;}
        public Guid? IdProjectLinked { get; set;}
        public string ProjectJiraKey { get; set;}
        public DateTime? WarrantyStartDate { get; set;}
        public DateTime? WarrantyEndDate { get; set;}
        public string ProjectTechnicalType { get; set;}
        public int? PriceType { get; set;}
        public double? Exchange { get; set;}
        public DateTime? ContractGuaranteeDate { get; set;}
        public DateTime? AdvanceGuaranteeDate { get; set;}
        public DateTime? WarrantyGuaranteeDate { get; set;}
        public double? ContractAmount { get; set;}
        public double? AdvanceAmount { get; set;}
        public double? WarrantyAmount { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Company_Title { get; set;}
        public string Corporation_Title { get; set;}
        public string ProjectType_Title { get; set;}
        public double? DayArea { get; set;}
        public int? ProjectStarEndArea { get; set;}
        public int? ProjectRemainingDay { get; set;}
        public string IsActive_Title { get; set;}
        public string IsConfirm_Title { get; set;}
        public string IdProjectLinked_Title { get; set;}
        public int? ProjectPersonCount { get; set;}
        public string PriceType_Title { get; set;}
        public string projectPersonIds { get; set;}
        public string searchField { get; set;}
    }
}
