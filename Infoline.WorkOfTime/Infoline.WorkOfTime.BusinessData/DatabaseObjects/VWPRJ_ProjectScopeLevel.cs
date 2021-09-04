using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ProjectScopeLevel : InfolineTable
    {
        public string level { get; set;}
        public Guid? projectId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string projectId_Title { get; set;}
        public string locationIds_Title { get; set;}
    }
}
