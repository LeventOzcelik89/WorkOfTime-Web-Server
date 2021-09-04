using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSRV_ServiceMaintenance : InfolineTable
    {
        public Guid? maintenanceStepId { get; set;}
        public Guid? serviceId { get; set;}
        public Guid? servicePID { get; set;}
        public string FormText { get; set;}
        public string FormJson { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Service_Title { get; set;}
        public string MaintenanceStep_Title { get; set;}
    }
}
