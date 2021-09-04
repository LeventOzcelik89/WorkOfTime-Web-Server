using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class DOC_DocumentRelation : InfolineTable
    {
        public Guid? documentId { get; set;}
        public Guid? documentRelationId { get; set;}
    }
}
