using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class FTM_TaskTemplateSubjectType : InfolineTable
    {
        public Guid? taskTemplateId { get; set;}
        public Guid? subjectId { get; set;}
    }
}
