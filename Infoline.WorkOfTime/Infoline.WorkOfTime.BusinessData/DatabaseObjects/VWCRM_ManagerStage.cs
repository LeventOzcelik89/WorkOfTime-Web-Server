using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_ManagerStage : InfolineTable
    {
        public string Name { get; set;}
        public string Code { get; set;}
        public string Description { get; set;}
        public string color { get; set;}
        public bool? IsSalesCompleted { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
