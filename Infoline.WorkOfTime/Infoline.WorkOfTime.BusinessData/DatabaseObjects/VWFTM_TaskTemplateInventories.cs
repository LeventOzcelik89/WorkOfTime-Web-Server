using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskTemplateInventories : InfolineTable
    {
        public Guid? taskTemplateId { get; set;}
        public Guid? inventoryId { get; set;}
        public string taskTemplateId_Title { get; set;}
        public string inventoryId_Title { get; set;}
    }
}
