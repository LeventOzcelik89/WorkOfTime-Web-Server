using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSRV_ServiceMaintenanceSteps : InfolineTable
    {
        public string FormText { get; set;}
        public string FormJson { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public int? FormCount { get; set;}
    }
}
