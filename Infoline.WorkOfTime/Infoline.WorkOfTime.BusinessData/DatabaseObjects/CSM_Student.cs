using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Aday öğrencilerin tutulduğu tablodur.
    /// </summary>
    public partial class CSM_Student : InfolineTable
    {
        /// <summary>
        /// Öğrencinin ad soyadının tutulduğu alandır.
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// Öğrencinin telefon numarasının tutulduğu alandır.
        /// </summary>
        public string phone { get; set;}
        /// <summary>
        /// Öğrencinin email adresinin tutulduğu alandır.
        /// </summary>
        public string email { get; set;}
        /// <summary>
        /// Öğrencinin iletişim izninin tutulduğu alandır.
        /// </summary>
        public bool? isAllowContact { get; set;}
        /// <summary>
        /// Öğrencinin okul bilgisinin tutulduğu alandır.
        /// </summary>
        public Guid? schoolId { get; set;}
        /// <summary>
        /// Öğrencinin sınıf bilgisinin tutulduğu alandır.
        /// </summary>
        public short? grade { get; set;}
        /// <summary>
        /// Öğrencinin verinin nerden geldiğinin tutulduğu alandır.
        /// </summary>
        public short? source { get; set;}
        /// <summary>
        /// Okulun konum bilgisinin tutulduğu alandır.
        /// </summary>
        public Guid? locationId { get; set;}
        /// <summary>
        /// Kaynak açıklaması
        /// </summary>
        public string sourceDescription { get; set;}
        /// <summary>
        /// Lisede öğrenim gördüğü bölüm
        /// </summary>
        public string deparmentCurrent { get; set;}
    }
}
