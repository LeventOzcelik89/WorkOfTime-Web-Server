using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class DOC_DocumentRevisionRequest : InfolineTable
    {
        public Guid? documentId { get; set;}
        public Guid? revisionUserId { get; set;}
        public string revisionNumber { get; set;}
        public string revisionContent { get; set;}
    }
}
