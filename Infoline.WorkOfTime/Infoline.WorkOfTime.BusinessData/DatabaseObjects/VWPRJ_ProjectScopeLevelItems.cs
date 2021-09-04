using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ProjectScopeLevelItems : InfolineTable
    {
        public Guid? projectId { get; set;}
        public Guid? scopeLevelId { get; set;}
        public Guid? locationId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string projectId_Title { get; set;}
        public string scopeLevelId_Title { get; set;}
        public string locationId_Title { get; set;}
    }
}
