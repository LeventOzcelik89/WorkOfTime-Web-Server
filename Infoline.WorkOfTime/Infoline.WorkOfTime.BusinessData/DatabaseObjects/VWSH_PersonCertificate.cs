using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PersonCertificate : InfolineTable
    {
        public Guid? CertificateTypeId { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public Guid? UserId { get; set;}
        public string CertificateName { get; set;}
        public int? CertificateTime { get; set;}
        public DateTime? ExpirationDate { get; set;}
        public double? point { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string CertificateType_Title { get; set;}
        public string User_Title { get; set;}
    }
}
