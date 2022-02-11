using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_WorkAccident : InfolineTable
    {
        public Guid? taskId { get; set;}
        public Guid? projectId { get; set;}
        public Guid? userId { get; set;}
        public string keyword { get; set;}
        public DateTime? accidentDate { get; set;}
        public Guid? templateId { get; set;}
        public string content { get; set;}
        public string code { get; set;}
    }
}
