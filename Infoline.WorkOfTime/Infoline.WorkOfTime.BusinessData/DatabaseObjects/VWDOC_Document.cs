using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWDOC_Document : InfolineTable
    {
        public string title { get; set;}
        public string subject { get; set;}
        public string code { get; set;}
        public short? type { get; set;}
        public Guid? responsibleUserId { get; set;}
        public string versionNumber { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string responsibleUserId_Title { get; set;}
        public string type_Title { get; set;}
        public int? totalRevision { get; set;}
        public string searchField { get; set;}
    }
}
