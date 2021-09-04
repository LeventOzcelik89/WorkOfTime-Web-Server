using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Görev Ataması Yapılan Personelleri tutan tablodur.
    /// </summary>
    public partial class FTM_TaskUser : InfolineTable
    {
        /// <summary>
        /// Görev Id sinin tutulduğu alandır.
        /// </summary>
        public Guid? taskId { get; set;}
        /// <summary>
        /// Kullanıcı Id sinin tutulduğu alandır.
        /// </summary>
        public Guid? userId { get; set;}
        /// <summary>
        /// Doğrulama kodunun tutulduğu alandır.
        /// </summary>
        public string verifyCode { get; set;}
        /// <summary>
        /// Üstlenme durumunun tutulduğu alandır.
        /// </summary>
        public bool? status { get; set;}
    }
}
