using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductionProduct : InfolineTable
    {
        public Guid? productionId { get; set;}
        public Guid? productId { get; set;}
        public string serialCodes { get; set;}
        public double? quantity { get; set;}
        public double? totalQuantity { get; set;}
        public double? amountSpent { get; set;}
        public double? price { get; set;}
        public Guid? materialId { get; set;}
        public short? type { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
		public string materialId_Title { get; set; }
	}
}
