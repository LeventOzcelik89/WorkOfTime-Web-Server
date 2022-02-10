using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_WorkAccidentCertificate : InfolineTable
    {
        public Guid? workAccidentId { get; set;}
        public Guid? personCertificateId { get; set;}
    }
}
