using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSV_ServiceOperation : InfolineTable
    {
        public Guid? serviceId { get; set;}
        public short? status { get; set;}
        public string description { get; set;}
        public Guid? CargoId { get; set;}
        public Guid? CompanyId { get; set;}
        public Guid? CargoNo { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string serviceId_Title { get; set;}
        public string cargoId_Title { get; set;}
        public string companyId_Title { get; set;}
    }
}
