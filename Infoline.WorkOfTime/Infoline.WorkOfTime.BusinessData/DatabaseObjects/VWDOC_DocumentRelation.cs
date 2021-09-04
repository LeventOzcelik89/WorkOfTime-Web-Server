using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWDOC_DocumentRelation : InfolineTable
    {
        public Guid? documentId { get; set;}
        public Guid? documentRelationId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string documentRelationCode { get; set;}
        public string documentRelationTitle { get; set;}
        public string documentRelationSubject { get; set;}
    }
}
