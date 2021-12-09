using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Görev operasyonlarının tutulduğu  tablodur.
    /// </summary>
    public partial class FTM_TaskOperation : InfolineTable
    {
        /// <summary>
        /// Operasyonun bağlı olduğu Görevin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? taskId { get; set;}
        /// <summary>
        /// Operasyonu gerçekleştiren kullanıcının idsinin tutulduğu alandır.
        /// </summary>
        public Guid? userId { get; set;}
        /// <summary>
        /// Operasyonun durumunun tutulduğu alandır.
        /// </summary>
        public int? status { get; set;}
        /// <summary>
        /// Operasyon için girilien açıklamının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Operasyonun gerçekleştirildiğinin konumun tutulduğu alandır.
        /// </summary>
        public IGeometry  location { get; set;}
        /// <summary>
        /// Operasyon envanterinin tutulduğu alandır.
        /// </summary>
        public Guid? fixtureId { get; set;}
        /// <summary>
        /// Operasyon anında telefonun batarya yüzdesi.
        /// </summary>
        public double? battery { get; set;}
        public short? subject { get; set;}
        public Guid? dataId { get; set;}
    }
}
