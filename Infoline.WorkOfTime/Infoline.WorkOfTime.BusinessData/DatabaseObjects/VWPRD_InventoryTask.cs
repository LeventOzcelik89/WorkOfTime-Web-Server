using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_InventoryTask : InfolineTable
    {
        public Guid? inventoryId { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public double? period { get; set;}
        public int? type { get; set;}
        public string description { get; set;}
        public Guid? userId { get; set;}
        public Guid? companyId { get; set;}
        public string period_Title { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string inventoryId_Title { get; set;}
        public string companyId_Title { get; set;}
        public string userId_Title { get; set;}
        public DateTime? lastTaskDate { get; set;}
    }
}
