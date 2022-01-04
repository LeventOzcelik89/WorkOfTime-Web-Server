using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSV_DeviceProblem : InfolineTable
    {
        public Guid? problemTypeId { get; set;}
        public Guid? serviceId { get; set;}
        public Guid? productId { get; set;}
        public string description { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string serviceId_Title { get; set;}
        public string problemTypeId_Title { get; set;}
    }
}
