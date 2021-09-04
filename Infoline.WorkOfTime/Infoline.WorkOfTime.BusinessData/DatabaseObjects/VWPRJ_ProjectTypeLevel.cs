using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ProjectTypeLevel : InfolineTable
    {
        public string level { get; set;}
        public short? type { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string projectId_Title { get; set;}
        public string type_Title { get; set;}
        public Guid? projectId { get; set;}
    }
}
