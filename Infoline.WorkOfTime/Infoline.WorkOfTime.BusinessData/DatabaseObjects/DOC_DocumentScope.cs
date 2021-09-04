using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class DOC_DocumentScope : InfolineTable
    {
        public Guid? documentId { get; set;}
        public Guid? organizationUnitId { get; set;}
    }
}
