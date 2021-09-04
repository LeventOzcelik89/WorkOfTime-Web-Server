using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class DOC_DocumentRevisionRequestConfirmation : InfolineTable
    {
        public Guid? documentId { get; set;}
        public Guid? revisionRequestId { get; set;}
        public Guid? confirmUserId { get; set;}
        public DateTime? date { get; set;}
        public short? status { get; set;}
        public string description { get; set;}
        public short? order { get; set;}
    }
}
