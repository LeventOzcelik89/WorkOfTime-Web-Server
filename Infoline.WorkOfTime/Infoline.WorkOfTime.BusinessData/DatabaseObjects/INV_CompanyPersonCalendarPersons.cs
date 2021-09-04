using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CompanyPersonCalendarPersons : InfolineTable
    {
        /// <summary>
        /// Company Person Id
        /// </summary>
        public Guid? IdUser { get; set;}
        /// <summary>
        /// INV_CompanyPersonCalendar.id alanı
        /// </summary>
        public Guid? IDPersonCalendar { get; set;}
    }
}
