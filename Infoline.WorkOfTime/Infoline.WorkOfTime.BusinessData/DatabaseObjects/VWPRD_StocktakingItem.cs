using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_StocktakingItem : InfolineTable
    {
        public Guid? stocktakingId { get; set;}
        public Guid? productId { get; set;}
        public string serialNumber { get; set;}
        public Single? quantity { get; set;}
        public Guid? unitId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string stocktakingId_Title { get; set;}
        public string productId_Title { get; set;}
        public string unitId_Title { get; set;}
    }
}
