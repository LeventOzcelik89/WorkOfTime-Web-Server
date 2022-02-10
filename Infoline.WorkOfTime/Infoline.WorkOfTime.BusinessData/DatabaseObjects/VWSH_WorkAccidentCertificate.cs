using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_WorkAccidentCertificate : InfolineTable
    {
        public Guid? workAccidentId { get; set;}
        public Guid? personCertificateId { get; set;}
    }
}
