using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class FTM_TaskSubjectType : InfolineTable
    {
        public Guid? taskId { get; set;}
        public Guid? subjectId { get; set;}
    }
}
