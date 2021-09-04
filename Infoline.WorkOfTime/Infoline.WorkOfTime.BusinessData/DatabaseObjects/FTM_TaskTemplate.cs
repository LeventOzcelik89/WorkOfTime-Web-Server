using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Saha görevlerinin tutulduğu tablodur.
    /// </summary>
    public partial class FTM_TaskTemplate : InfolineTable
    {
        /// <summary>
        /// Görev kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        public string name { get; set;}
        /// <summary>
        /// Görev tipinin tutulduğu alandır.(Arıza,Bakım,Kalibrasyon,Parça Değişikliği)
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Görevin  atamasının yapıldığı şirketin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? companyId { get; set;}
        /// <summary>
        /// Görev lokasyonunun tutulduğu alandır.
        /// </summary>
        public IGeometry  location { get; set;}
        public short? priority { get; set;}
        /// <summary>
        /// Görevin açıldığı müşteri idsinin tutulduğu alandır.
        /// </summary>
        public Guid? customerId { get; set;}
        public Guid? customerStorageId { get; set;}
        public Guid? companyCarId { get; set;}
        public int? estimatedTaskMinute { get; set;}
        public string description { get; set;}
        /// <summary>
        /// Görevin Verifycode ile atama yapılıp yapılmayacağını tutan alandır.
        /// </summary>
        public bool? hasVerifyCode { get; set;}
        public bool? sendMail { get; set;}
    }
}
