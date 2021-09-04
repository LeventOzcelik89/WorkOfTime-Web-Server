using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_PermitType : InfolineTable
    {
        public string Name { get; set;}
        public bool? IsPaidPermit { get; set;}
        public int? PaidPermitDay { get; set;}
        public bool? CanHourly { get; set;}
        public string Descriptions { get; set;}
        public bool? BeDocumented { get; set;}
        public bool? RequestStaff { get; set;}
        public bool? ViewStaff { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string IsPaidPermit_Title { get; set;}
        public string CanHourly_Title { get; set;}
        public string BeDocumented_Title { get; set;}
    }
}
