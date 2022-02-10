using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_CorrectiveActivity : InfolineTable
    {
        public Guid? taskId { get; set;}
        public Guid? workAccidentId { get; set;}
        public Guid? projectId { get; set;}
        public Guid? userId { get; set;}
        public string keyword { get; set;}
        public DateTime? date { get; set;}
        public Guid? templateId { get; set;}
        public string content { get; set;}
    }
}
