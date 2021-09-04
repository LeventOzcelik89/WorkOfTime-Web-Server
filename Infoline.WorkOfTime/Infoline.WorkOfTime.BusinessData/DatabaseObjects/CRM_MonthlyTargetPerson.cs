using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_MonthlyTargetPerson : InfolineTable
    {
        /// <summary>
        /// Ay yıl olarak dönemin kayıt edildiği alandır.
        /// </summary>
        public DateTime? CPeriod { get; set;}
        /// <summary>
        /// Personel ID alanı. SH_User.id ile eşleşir. Aylık Hedefin kime atandığı bilgisini tutar.
        /// </summary>
        public Guid? TargetUserId { get; set;}
        /// <summary>
        /// Ürün veya Ürün Grubu id sidir.
        /// </summary>
        public Guid? ProductGroupId { get; set;}
        /// <summary>
        /// Hedef Puandır.
        /// </summary>
        public int? TargetPoint { get; set;}
        /// <summary>
        /// Hedef Dip Puanın yüzdesidir.
        /// </summary>
        public int? TargetPercent { get; set;}
        /// <summary>
        /// Hediye edilen yüzdelik dilim.
        /// </summary>
        public int? BonusPercentage { get; set;}
        /// <summary>
        /// Fokus mu, işaretli ise fokus işaretli değil ise değildir. 0 veya null ise fokus değil, 1 ise fokus tur.
        /// </summary>
        public bool? IsFocus { get; set;}
        /// <summary>
        /// Toplu Focus mu ?
        /// </summary>
        public bool? IsMultipleFocus { get; set;}
        /// <summary>
        /// Toplu Focusların birden fazla olduğu durumları kontrol edebilmek için kullanılan alandır. Her satıra özel bir numara verilmesi işlemi.
        /// </summary>
        public Guid? RowId { get; set;}
        /// <summary>
        /// Aylık Hedefleri gruplayabilmek adına kullanılan, fonksiyonel bir alandır.
        /// </summary>
        public Guid? GroupId { get; set;}
    }
}
