using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PersonCertificateType : InfolineTable
    {
        public string CertificateName { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
