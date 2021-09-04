using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_ManagerStage : InfolineTable
    {
        public string Name { get; set;}
        public string Code { get; set;}
        public string Description { get; set;}
        public string color { get; set;}
        /// <summary>
        /// Satış İşlemi Tamamlanmış bir aşama mı ?
        /// </summary>
        public bool? IsSalesCompleted { get; set;}
    }
}
