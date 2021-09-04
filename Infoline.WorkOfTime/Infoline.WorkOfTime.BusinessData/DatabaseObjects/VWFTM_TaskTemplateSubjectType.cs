using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskTemplateSubjectType : InfolineTable
    {
        public Guid? taskTemplateId { get; set;}
        public Guid? subjectId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string subjectId_Title { get; set;}
    }
}
