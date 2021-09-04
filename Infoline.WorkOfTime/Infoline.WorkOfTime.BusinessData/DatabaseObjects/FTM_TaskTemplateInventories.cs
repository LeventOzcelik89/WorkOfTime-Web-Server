using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class FTM_TaskTemplateInventories : InfolineTable
    {
        public Guid? taskTemplateId { get; set;}
        public Guid? inventoryId { get; set;}
    }
}
