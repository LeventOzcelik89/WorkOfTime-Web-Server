using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWDOC_DocumentRevisionRequest : InfolineTable
    {
        public Guid? documentId { get; set;}
        public Guid? revisionUserId { get; set;}
        public string revisionNumber { get; set;}
        public string revisionContent { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string documentCode { get; set;}
        public string documentTitle { get; set;}
        public string documentSubject { get; set;}
        public short? lastStatus { get; set;}
        public string confirmationUserIds { get; set;}
        public string confirmationUserTitles { get; set;}
        public string lastStatus_Title { get; set;}
    }
}
