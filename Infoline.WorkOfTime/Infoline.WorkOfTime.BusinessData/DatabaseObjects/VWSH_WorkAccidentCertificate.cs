using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_WorkAccidentCertificate : InfolineTable
    {
        public Guid? workAccidentId { get; set;}
        public Guid? personCertificateId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public Guid? CertificateTypeId { get; set;}
        public string CertificateTypeId_Title { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public Guid? userId { get; set;}
        public string userId_Title { get; set;}
        public string certificateName { get; set;}
        public int? certificateTime { get; set;}
        public DateTime? expirationDate { get; set;}
        public double? point { get; set;}
    }
}
