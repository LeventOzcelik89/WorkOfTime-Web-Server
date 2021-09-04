using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PersonLanguages : InfolineTable
    {
        public Guid? UserId { get; set;}
        public int? Languages { get; set;}
        public int? Reads { get; set;}
        public int? Write { get; set;}
        public int? Speak { get; set;}
        public Guid? CertificateTypeId { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public string CertificateName { get; set;}
        public int? CertificateTime { get; set;}
        public DateTime? ExpirationDate { get; set;}
        public double? point { get; set;}
        public string created_Title { get; set;}
        public string changedby_Title { get; set;}
        public string User_Title { get; set;}
        public string Languages_Title { get; set;}
        public string Reads_Title { get; set;}
        public string Write_Title { get; set;}
        public string Speak_Title { get; set;}
        public string CertificateType_Title { get; set;}
    }
}
