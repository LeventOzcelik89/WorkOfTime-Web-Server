using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductMateriel : InfolineTable
    {
        public Guid? productId { get; set;}
        public double? quantity { get; set;}
        public Guid? materialId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string materialId_Title { get; set;}
        public string unitId_Title { get; set;}
        public double? price { get; set;}
        public double? stockTotal { get; set;}
        public string stockCompanyIds { get; set;}
    }
}
