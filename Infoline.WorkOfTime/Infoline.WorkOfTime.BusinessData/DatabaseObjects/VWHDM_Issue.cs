using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHDM_Issue : InfolineTable
    {
        public Guid? pid { get; set;}
        public short? status { get; set;}
        public string title { get; set;}
        public int? expiryMinute { get; set;}
        public Guid? mainId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string pid_Title { get; set;}
        public string status_Title { get; set;}
    }
}
