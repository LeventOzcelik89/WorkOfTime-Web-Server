using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class DOC_Document : InfolineTable
    {
        public string code { get; set;}
        public string title { get; set;}
        public string subject { get; set;}
        public short? type { get; set;}
        public Guid? responsibleUserId { get; set;}
        public string versionNumber { get; set;}
    }
}
