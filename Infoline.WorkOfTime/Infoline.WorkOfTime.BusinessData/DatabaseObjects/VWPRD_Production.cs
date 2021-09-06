using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_Production : InfolineTable
    {
        public string name { get; set;}
        public string description { get; set;}
        public DateTime? productionOrderDate { get; set;}
        public DateTime? scheduledProductionDate { get; set;}
        public Guid? productionCompanyId { get; set;}
        public Guid? productionStorageId { get; set;}
        public Guid? centerCompanyId { get; set;}
        public Guid? centerStorageId { get; set;}
        public Guid? productId { get; set;}
        public double? quantity { get; set;}
        public string code { get; set;}
        public DateTime? lastProductionDate { get; set;}
        public string schemaTitle { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productionCompanyId_Title { get; set;}
        public string productionStorageId_Title { get; set;}
        public string centerCompanyId_Title { get; set;}
        public string centerStorageId_Title { get; set;}
		public string lastOperationStatus_Title { get; set; }
		public string productId_Title { get; set; }
		public int lastOperationStatus { get; set; }
		public DateTime? lastOperationDate { get; set; }
	}
}
