using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Stok sayım işlemlerinin tutulduğu tablodur.
    /// </summary>
    public partial class PRD_Stocktaking : InfolineTable
    {
        /// <summary>
        /// Sayım kodunun tutulduğu alandır.
        /// </summary>
        public string code { get; set;}
        /// <summary>
        /// Sayım tarihinin tutulduğu alandır.
        /// </summary>
        public DateTime? date { get; set;}
        /// <summary>
        /// Sayım yapılan deponun id sinin tutulduğu alandır.
        /// </summary>
        public Guid? storageId { get; set;}
        /// <summary>
        /// Sayımın sorumlu personelinin id sinin tutulduğu alandır.
        /// </summary>
        public Guid? responsibleUserId { get; set;}
        /// <summary>
        /// İşlem açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// Sayım işleminin durumunun tutulduğu alandır.
        /// </summary>
        public short? status { get; set;}
    }
}
