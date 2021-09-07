using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductionOperation : InfolineTable
    {
        public Guid? productionId { get; set;}
        public short? status { get; set;}
        public string description { get; set;}
        public Guid? userId { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string dataId_Title { get; set;}
        public string status_Title { get; set;}
    }
}
