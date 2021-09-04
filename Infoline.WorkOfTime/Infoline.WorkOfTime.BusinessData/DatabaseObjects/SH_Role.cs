using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_Role : InfolineTable
    {
        public string rolname { get; set;}
        public string roledescription { get; set;}
        /// <summary>
        /// Sistem ve Kullanıcı Tanımlı
        /// </summary>
        public short? roletype { get; set;}
    }
}
