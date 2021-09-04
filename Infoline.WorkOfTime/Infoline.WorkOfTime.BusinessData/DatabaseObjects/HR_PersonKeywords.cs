using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class HR_PersonKeywords : InfolineTable
    {
        public Guid? HrPersonId { get; set;}
        public Guid? HrKeywordsId { get; set;}
    }
}
