using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSYS_TableAdditionalProperty : InfolineTable
    {
        public string dataTable { get; set;}
        public Guid? dataId { get; set;}
        public string propertyKey { get; set;}
        public string propertyValue { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
