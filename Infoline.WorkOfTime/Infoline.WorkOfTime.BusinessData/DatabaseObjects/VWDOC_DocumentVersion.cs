using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWDOC_DocumentVersion : InfolineTable
    {
        public Guid? documentId { get; set;}
        public string versionNumber { get; set;}
        public string content { get; set;}
        public bool? isActive { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string documentCode { get; set;}
    }
}
