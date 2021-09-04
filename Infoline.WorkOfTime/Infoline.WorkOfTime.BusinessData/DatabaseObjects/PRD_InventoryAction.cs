using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Envanter Hareketlerinin Tutulduğu Tablodur.
    /// </summary>
    public partial class PRD_InventoryAction : InfolineTable
    {
        /// <summary>
        /// İşlem idsinin tutulduğu alandır.
        /// </summary>
        public Guid? transactionId { get; set;}
        /// <summary>
        /// İşlem idsinin tutulduğu alandır.
        /// </summary>
        public Guid? transactionItemId { get; set;}
        /// <summary>
        /// Envanter idsinin tutulduğu alandır.
        /// </summary>
        public Guid? inventoryId { get; set;}
        /// <summary>
        /// Hareket Tipinin tutulduğu alandır.
        /// </summary>
        public short? type { get; set;}
        /// <summary>
        /// Kayıt yapılacak yerin idsinin tutulduğu alandır.
        /// </summary>
        public Guid? dataId { get; set;}
        /// <summary>
        /// Kayıt yapılacak yerin tablosunun tutulduğu alandır.
        /// </summary>
        public string dataTable { get; set;}
        /// <summary>
        /// Kayıt yapılacak yerin carisinin tutulduğu alandır.
        /// </summary>
        public Guid? dataCompanyId { get; set;}
        /// <summary>
        /// Konum bilgisinin tutulduğu alandır.
        /// </summary>
        public IGeometry  location { get; set;}
    }
}
