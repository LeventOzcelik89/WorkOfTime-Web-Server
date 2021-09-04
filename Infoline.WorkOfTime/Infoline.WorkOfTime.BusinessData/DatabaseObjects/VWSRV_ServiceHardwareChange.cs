using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSRV_ServiceHardwareChange : InfolineTable
    {
        public Guid? serviceId { get; set;}
        public Guid? servicePID { get; set;}
        public int? changeType { get; set;}
        public string description { get; set;}
        public Guid? fixtureId { get; set;}
        public string FilePath { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string changeType_Title { get; set;}
        public string fixtureId_Title { get; set;}
    }
}
