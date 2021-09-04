using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_StaffNeedsKeywords : InfolineTable
    {
        public Guid? HrKeywordsId { get; set;}
        public Guid? HrStaffNeedsId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string HrKeywords_Title { get; set;}
    }
}
