using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_StockAction : InfolineTable
    {
        public int direction { get; set;}
        public string currency_Title { get; set;}
        public Guid? transactionId { get; set;}
        public Guid? productId { get; set;}
        public double? unitPrice { get; set;}
        public string serialCodes { get; set;}
        public Guid? unitId { get; set;}
        public double? quantity { get; set;}
        public Guid? alternativeUnitId { get; set;}
        public double? alternativeQuantity { get; set;}
        public string code { get; set;}
        public DateTime? date { get; set;}
        public short? type { get; set;}
        public short? status { get; set;}
        public Guid? stockId { get; set;}
        public string stockTable { get; set;}
        public Guid? stockCompanyId { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public Guid? dataCompanyId { get; set;}
        public Guid? tritemUnitId { get; set;}
        public string createdby_Title { get; set;}
        public string stockId_Title { get; set;}
        public string stockCompanyId_Title { get; set;}
        public string dataId_Title { get; set;}
        public string dataCompanyId_Title { get; set;}
        public string productId_Title { get; set;}
        public string productName { get; set;}
        public string productCode { get; set;}
        public string status_Title { get; set;}
        public string type_Title { get; set;}
        public string unitId_Title { get; set;}
        public string alternativeUnitId_Title { get; set;}
        public double? totalPrice { get; set;}
        public double? totalQuantity { get; set;}
    }
}
