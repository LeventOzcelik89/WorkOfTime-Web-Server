using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_Permit : InfolineTable
    {
        /// <summary>
        /// Otomatik İzin Kodu
        /// </summary>
        public string PermitCode { get; set;}
        /// <summary>
        /// Onay Durum Bilgileri Tutan Enum alanıdır. 0 Talep Edildi, 1 1. Onay, 2 2. Onay , 98 1. Red,  99 2. Red
        /// </summary>
        public int? ApproveStatus { get; set;}
        /// <summary>
        /// Yöneticilerden herhangi birisi reddeder ise açıklama girmesi için gerekli alan
        /// </summary>
        public string ApproveDetail { get; set;}
        /// <summary>
        /// Permit Type Tablosunun id alanını tutar
        /// </summary>
        public Guid? PermitTypeId { get; set;}
        /// <summary>
        /// İzin Başlangıç Süresi
        /// </summary>
        public DateTime? StartDate { get; set;}
        /// <summary>
        /// İzin Bitiş Süresi
        /// </summary>
        public DateTime? EndDate { get; set;}
        /// <summary>
        /// Ulaşılacak Telefon Numarası
        /// </summary>
        public string AccessPhone { get; set;}
        public string ArriveAdress { get; set;}
        public string Detail { get; set;}
        public DateTime? Manager1ApprovalDate { get; set;}
        public DateTime? Manager2ApprovalDate { get; set;}
        public Guid? Manager1Approval { get; set;}
        public Guid? Manager2Approval { get; set;}
        public DateTime? IkApprovalDate { get; set;}
        public Guid? IkApproval { get; set;}
        public double? TotalDays { get; set;}
        public Guid? IdUser { get; set;}
        public double? TotalHours { get; set;}
        public string PersonToLook1 { get; set;}
        public string PersonToLook2 { get; set;}
        public string PersonToLook3 { get; set;}
    }
}
