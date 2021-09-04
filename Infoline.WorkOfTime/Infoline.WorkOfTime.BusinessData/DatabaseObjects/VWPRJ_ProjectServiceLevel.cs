using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ProjectServiceLevel : InfolineTable
    {
        public Guid? scopeLevelId { get; set;}
        public short? amercement { get; set;}
        public short? endTime { get; set;}
        public bool? isWorkingTime { get; set;}
        public short? resolutionType { get; set;}
        public bool? nextDay { get; set;}
        public Guid? projectId { get; set;}
        public short? taskType { get; set;}
        public Guid? typeLevelId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string projectId_Title { get; set;}
        public string scopeLevelId_Title { get; set;}
        public string Level_Title { get; set;}
        public string resolutionType_Title { get; set;}
        public string type_Title { get; set;}
        public short? resolutionTime { get; set;}
        public short? startTime { get; set;}
    }
}
