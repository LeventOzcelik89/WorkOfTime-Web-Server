using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Şirket personellerinin işe giriş cıkış çalıştığı süreleri tutan tablodur
    /// </summary>
    public partial class INV_CompanyPerson : InfolineTable
    {
        /// <summary>
        /// INV_Company tablosundaki id ile eşleşir. Şirketin eşsiz kodudur.
        /// </summary>
        public Guid? CompanyId { get; set;}
        /// <summary>
        /// SH_User tablosundaki id alanı ile eşleşir. İşletmede çalışan personel id alanıdır. 
        /// </summary>
        public Guid? IdUser { get; set;}
        /// <summary>
        /// işe giriş tarihi
        /// </summary>
        public DateTime? JobStartDate { get; set;}
        /// <summary>
        /// işten çıkış tarihi
        /// </summary>
        public DateTime? JobEndDate { get; set;}
        /// <summary>
        /// İşten Ayrılma Sebebi : ENUM => 0: İşletme tarafından işe son verme, 1: İstifa, 2: Zoraki Koşullar ( Askerlik vb ), 3: Diğer
        /// </summary>
        public int? JobLeaving { get; set;}
        /// <summary>
        /// İşten ayrılma sebebi açıklaması
        /// </summary>
        public string JobLeavingDescription { get; set;}
        /// <summary>
        /// Sigortadaki Ünvanı
        /// </summary>
        public string Title { get; set;}
        public int? Level { get; set;}
    }
}
