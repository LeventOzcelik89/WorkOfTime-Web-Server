using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Saha görevlerinin tutulduğu tablodur.
    /// </summary>
    public partial class FTM_Task : InfolineTable
    {
        /// <summary>
        /// Görev kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Görev tipinin tutulduğu alandır.(Arıza,Bakım,Kalibrasyon,Parça Değişikliği)
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Görevin  atamasının yapıldığı şirketin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? companyId { get; set;}
        /// <summary>
        /// Görevin Verifycode ile atama yapılıp yapılmayacağını tutan alandır.
        /// </summary>
        public bool? hasVerifyCode { get; set;}
        /// <summary>
        /// Görev envanterinin tutulduğu alandır.
        /// </summary>
        public Guid? fixtureId { get; set;}
        /// <summary>
        /// Görev lokasyonunun tutulduğu alandır.
        /// </summary>
        public IGeometry  location { get; set;}
        public short? priority { get; set;}
        /// <summary>
        /// Görevin açıldığı müşteri idsinin tutulduğu alandır.
        /// </summary>
        public Guid? customerId { get; set;}
        /// <summary>
        /// Görevin planlanan bitiş tarihi
        /// </summary>
        public DateTime? dueDate { get; set;}
        public Guid? customerStorageId { get; set;}
        public DateTime? planStartDate { get; set;}
        public DateTime? notificationDate { get; set;}
        public Guid? companyCarId { get; set;}
        /// <summary>
        /// Hangi Plan aracılığıyla oluşturuldu bu görev.
        /// </summary>
        public Guid? taskPlanId { get; set;}
        /// <summary>
        /// Hangi Görev Template aracılığıyla oluşturuldu bu görev.
        /// </summary>
        public Guid? taskTemplateId { get; set;}
        public short? planLater { get; set;}
        public short? sendMailCustomer { get; set;}
        public string sendedCustomer { get; set;}
    }
}
