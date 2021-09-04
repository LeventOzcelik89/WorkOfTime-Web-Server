using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_Commissions : InfolineTable
    {
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        /// <summary>
        /// Görevlendirme Bilgileri, Enum, 0: Denetim 1: Eğitim 2: Satış 3: Organizasyon 4: Teknik Destek 5: Satın Alma 6: Davet 7: Diğer
        /// </summary>
        public int? Information { get; set;}
        /// <summary>
        /// Diğer: Görevlendirme Bilgisi "Diğer" Seçildi ise girilecek olan detay inputu.
        /// </summary>
        public string InformationDetail { get; set;}
        /// <summary>
        /// Seyahat Bilgileri, 0: Araç 1: Otobüs 2: Uçak 3 Diğer
        /// </summary>
        public int? TravelInformation { get; set;}
        /// <summary>
        /// Diğer: Seyahat Bilgisi "Diğer" Seçildi ise girilecek olan detay inputu.
        /// </summary>
        public string TravelInformationDetail { get; set;}
        /// <summary>
        /// Açıklamalar
        /// </summary>
        public string Descriptions { get; set;}
        public DateTime? Manager1ApprovalDate { get; set;}
        public Guid? Manager1Approval { get; set;}
        /// <summary>
        /// Onay Durum Bilgileri Tutan Enum alanıdır. 0 Talep Edildi, 1 1. Onay, 2 2. Onay , 98 1. Red,  99 2. Red
        /// </summary>
        public int? ApproveStatus { get; set;}
        public string CommissionCode { get; set;}
        public string ToCorporation { get; set;}
        public string ToAdress { get; set;}
        public string CommissionSubject { get; set;}
        public double? TotalDays { get; set;}
        public double? TotalHours { get; set;}
        public int? RequestForAccommodation { get; set;}
        public Guid? IdCompanyCar { get; set;}
        public string VehiclePlate { get; set;}
    }
}
