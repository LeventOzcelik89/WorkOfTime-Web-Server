using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRJ_ProjectSubjectLevelItems : InfolineTable
    {
        public Guid? subjectId { get; set;}
        public Guid? projectId { get; set;}
        public Guid? subjectLevelId { get; set;}
    }
}
