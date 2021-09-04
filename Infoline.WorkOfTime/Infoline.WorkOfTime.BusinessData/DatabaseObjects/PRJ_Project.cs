using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRJ_Project : InfolineTable
    {
        /// <summary>
        /// Proje isminin tutulduğu alandır.
        /// </summary>
        public string ProjectName { get; set;}
        /// <summary>
        /// Proje kodunun tutulduğu alandır.
        /// </summary>
        public string ProjectCode { get; set;}
        /// <summary>
        /// Proje faaliyet alanının tutulduğu alandır.
        /// </summary>
        public string ProjectScope { get; set;}
        /// <summary>
        /// Proje başlangıç tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? ProjectStartDate { get; set;}
        /// <summary>
        /// Proje bitiş tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? ProjectEndDate { get; set;}
        /// <summary>
        /// Proje onay tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? ProjectConfirmDate { get; set;}
        /// <summary>
        /// Projenin Onay Durumunun saklandığı alandır.
        /// </summary>
        public bool? IsConfirm { get; set;}
        /// <summary>
        /// Projenin Aktiflik Durumunun saklandığı alandır.
        /// </summary>
        public bool? IsActive { get; set;}
        /// <summary>
        /// ENUM > YAZILIM, DONANIM, HİZMET
        /// </summary>
        public int? ProjectType { get; set;}
        public Guid? CompanyId { get; set;}
        public Guid? CorporationId { get; set;}
        /// <summary>
        /// İhale No
        /// </summary>
        public string TenderNo { get; set;}
        /// <summary>
        /// İhale adi
        /// </summary>
        public string TenderName { get; set;}
        /// <summary>
        /// ihale yazısı
        /// </summary>
        public string TenderWrite { get; set;}
        public Guid? IdProjectLinked { get; set;}
        public string ProjectJiraKey { get; set;}
        public DateTime? WarrantyStartDate { get; set;}
        public DateTime? WarrantyEndDate { get; set;}
        public string ProjectTechnicalType { get; set;}
        /// <summary>
        /// Fiyat Tipi
        /// </summary>
        public int? PriceType { get; set;}
        /// <summary>
        /// Kur
        /// </summary>
        public double? Exchange { get; set;}
        /// <summary>
        /// Sözleşme Teminat Tarihi
        /// </summary>
        public DateTime? ContractGuaranteeDate { get; set;}
        /// <summary>
        /// Avans Teminat Tarihi
        /// </summary>
        public DateTime? AdvanceGuaranteeDate { get; set;}
        /// <summary>
        /// Garanti Teminat Tarihi
        /// </summary>
        public DateTime? WarrantyGuaranteeDate { get; set;}
        public double? ContractAmount { get; set;}
        public double? AdvanceAmount { get; set;}
        public double? WarrantyAmount { get; set;}
    }
}
