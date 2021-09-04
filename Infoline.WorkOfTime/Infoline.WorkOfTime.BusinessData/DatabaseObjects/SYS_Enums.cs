using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Sistem tablosudur. Kod içinde bulunan tüm enum classlarının verilerinin tutulduğu tablodur.
    /// </summary>
    public partial class SYS_Enums : InfolineTable
    {
        /// <summary>
        /// Enum adının tutulduğu alandır.
        /// </summary>
        public string Name { get; set;}
        /// <summary>
        /// Enum'ın kullanıldığı tablo adının tutulduğu alandır.
        /// </summary>
        public string tableName { get; set;}
        /// <summary>
        /// Enum'a ait alan adının tutulduğu alandır.
        /// </summary>
        public string fieldName { get; set;}
        /// <summary>
        /// Enum'a ait key değeri
        /// </summary>
        public int? enumKey { get; set;}
        /// <summary>
        /// Enum'a ait değer tanımı
        /// </summary>
        public string enumValue { get; set;}
        /// <summary>
        /// Enum'ın açıklaması
        /// </summary>
        public string enumDescription { get; set;}
    }
}
