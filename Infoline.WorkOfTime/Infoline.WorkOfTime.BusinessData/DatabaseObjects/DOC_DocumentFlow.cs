using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class DOC_DocumentFlow : InfolineTable
    {
        public Guid? documentId { get; set;}
        public short? type { get; set;}
        public Guid? organizationUnitId { get; set;}
        public Guid? userId { get; set;}
        public short? order { get; set;}
    }
}
