﻿using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_StaffNeedsRequester : InfolineTable
    {
        public Guid? RequestApprovedId { get; set;}
        public DateTime? RequestDate { get; set;}
        public Guid? LocationId { get; set;}
        public int? ArrivalType { get; set;}
        public int? Education { get; set;}
        public int? MilitaryStatus { get; set;}
        public int? Language { get; set;}
        public int? LanguageType { get; set;}
        public int? DriverLicense { get; set;}
        public int? ExprienceYear { get; set;}
        public Guid? ReferenceId { get; set;}
        public int? ReasonForStaffDemand { get; set;}
        public int? EmploymentStatus { get; set;}
        public int? Gender { get; set;}
        public int? MaritalStatus { get; set;}
        public int? Travelability { get; set;}
        public string Description { get; set;}
        public DateTime? RetirementDate { get; set;}
        public Guid? IkApproval { get; set;}
        public string NeedCode { get; set;}
        public int? SalaryRangeMin { get; set;}
        public int? SalaryRangeMax { get; set;}
        public string Comment { get; set;}
        public short? priority { get; set;}
        public short? lastStatus { get; set;}
        public Guid? RequestId { get; set;}
        public string LocationId_Title { get; set;}
        public string lastStatus_Title { get; set;}
        public string ReasonForStaffDemand_Title { get; set;}
        public string Education_Title { get; set;}
        public string DriverLicense_Title { get; set;}
        public string priority_Title { get; set;}
        public int? TotalYonlendirme { get; set;}
        public string HrPosition_Title { get; set;}
        public string HrKeywords_Title { get; set;}
    }
}