using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CompanyPersonAvailability : InfolineTable
    {
        public Guid? IdUser { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public Guid? IdProject { get; set;}
        public double? rate { get; set;}
        public bool? isSalary { get; set;}
        /// <summary>
        /// Personelin bakım içinmi yoksa Proje içinmi çalıştığı ayrımı
        /// 
        /// 
        /// </summary>
        public short? type { get; set;}
    }
}
