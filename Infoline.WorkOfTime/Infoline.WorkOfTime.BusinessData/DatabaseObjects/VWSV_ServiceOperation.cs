using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSV_ServiceOperation : InfolineTable
    {
        public Guid? serviceId { get; set;}
        public short? status { get; set;}
        public string description { get; set;}
        public Guid? userId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
        public string serviceId_Title { get; set;}
    }
}
