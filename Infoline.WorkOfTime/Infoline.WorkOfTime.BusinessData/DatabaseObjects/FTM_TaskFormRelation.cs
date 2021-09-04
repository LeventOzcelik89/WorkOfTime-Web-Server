using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Ürün&Envanter Görev Form tanımlarının tutulduğu tablodur.
    /// </summary>
    public partial class FTM_TaskFormRelation : InfolineTable
    {
        /// <summary>
        /// Envanter idsinin tutulduğu alandır.
        /// </summary>
        public Guid? inventoryId { get; set;}
        /// <summary>
        /// Ürün idsinin tutulduğu alandır.
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Form idsinin tutulduğu alandır.
        /// </summary>
        public Guid? formId { get; set;}
        public Guid? companyId { get; set;}
        public Guid? companyStorageId { get; set;}
    }
}
