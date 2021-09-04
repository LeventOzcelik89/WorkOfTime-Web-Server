using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductTask : InfolineTable
    {
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public double? period { get; set;}
        public int? type { get; set;}
        public string description { get; set;}
        public Guid? productId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
