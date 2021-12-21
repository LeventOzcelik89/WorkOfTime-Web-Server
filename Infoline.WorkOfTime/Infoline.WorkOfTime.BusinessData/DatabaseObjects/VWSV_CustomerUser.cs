using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSV_CustomerUser : InfolineTable
    {
        public string code { get; set;}
        public Guid? name { get; set;}
        public Guid? serviceId { get; set;}
        public Guid? customerId { get; set;}
        public short? type { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string service_Code { get; set;}
        public string customerId_FullName { get; set;}
    }
}
