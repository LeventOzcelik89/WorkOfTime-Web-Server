using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_CompanyBasedPrice : InfolineTable
    {
        /// <summary>
        /// Şirket
        /// </summary>
        public Guid? companyId { get; set;}
        /// <summary>
        /// Ürün
        /// </summary>
        public Guid? productId { get; set;}
        /// <summary>
        /// Kategori
        /// </summary>
        public Guid? categoryId { get; set;}
        /// <summary>
        /// Genel, Minimum Satış Tutarına göre, Minimum Fiyata Göre
        /// </summary>
        public short? conditionType { get; set;}
        /// <summary>
        /// Genel, Peşin, Taksitli, Vade
        /// </summary>
        public short? sellingType { get; set;}
    }
}
