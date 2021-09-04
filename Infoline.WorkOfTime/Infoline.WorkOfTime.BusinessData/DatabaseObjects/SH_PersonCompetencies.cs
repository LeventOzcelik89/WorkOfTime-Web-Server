using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_PersonCompetencies : InfolineTable
    {
        /// <summary>
        /// SH_User tablosundaki id ile eşleşir. Kullanıcının eşsiz kodudur.
        /// </summary>
        public Guid? UserId { get; set;}
        /// <summary>
        /// SH_Competencies tablosundaki id ile eşleşir. Kullanıcının eşsiz kodudur
        /// </summary>
        public Guid? CompetenciesId { get; set;}
        /// <summary>
        /// Yeterlilik seviyesi. 1-5 arası
        /// </summary>
        public int? CompetenciesLevel { get; set;}
    }
}
