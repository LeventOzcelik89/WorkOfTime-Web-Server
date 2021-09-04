using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_Presentation : InfolineTable
    {
        public string Name { get; set;}
        public Guid? SalesPersonId { get; set;}
        public Guid? ChannelCompanyId { get; set;}
        public Guid? CustomerCompanyId { get; set;}
        public Guid? PresentationStageId { get; set;}
        /// <summary>
        /// Taahhüt Bitiş Tarihi
        /// </summary>
        public DateTime? CommitmentEndDate { get; set;}
        /// <summary>
        /// Rakibin Toplam fatura tutarı
        /// </summary>
        public double? OpponentInvoiceAmount { get; set;}
        /// <summary>
        /// Vodafone Tarife Teklifimiz
        /// </summary>
        public double? VodafoneOffer { get; set;}
        /// <summary>
        /// Kapanma Yüzdesi
        /// </summary>
        public double? CompletionRate { get; set;}
        /// <summary>
        /// Tahmini Kapanma Tarihi
        /// </summary>
        public DateTime? EstimatedCompletionDate { get; set;}
        /// <summary>
        /// Bütçe
        /// </summary>
        public long? Budget { get; set;}
        /// <summary>
        /// Müşteri önem derecesinin tutulduğu alandır.
        /// </summary>
        public int? PriorityLevel { get; set;}
        public bool? hasContact { get; set;}
        public short? PlaceofArrival { get; set;}
    }
}
