using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_StaffNeedsStatus : InfolineTable
    {
        public Guid? staffNeedId { get; set;}
        public string description { get; set;}
        public short? status { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string staffNeedId_Title { get; set;}
        public string status_Title { get; set;}
    }
}
