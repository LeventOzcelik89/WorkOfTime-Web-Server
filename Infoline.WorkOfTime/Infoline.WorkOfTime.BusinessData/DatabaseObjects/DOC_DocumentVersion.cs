using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class DOC_DocumentVersion : InfolineTable
    {
        public Guid? documentId { get; set;}
        public string versionNumber { get; set;}
        public string content { get; set;}
        public bool? isActive { get; set;}
    }
}
