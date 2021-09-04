using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_Product : InfolineTable
    {
        public double stockPerson { get; set;}
        public double stockStorage { get; set;}
        public double stockTotal { get; set;}
        public int criticalStockAlert { get; set;}
        public string statusFlags { get; set;}
        public string fullName { get; set;}
        public string code { get; set;}
        public string barcode { get; set;}
        public string name { get; set;}
        public string description { get; set;}
        public Guid? categoryId { get; set;}
        public Guid? brandId { get; set;}
        public Guid? unitId { get; set;}
        public short? type { get; set;}
        public short? status { get; set;}
        public short? barcodeType { get; set;}
        public bool? isCriticalStock { get; set;}
        public int? criticalStock { get; set;}
        public short? stockType { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string categoryId_Title { get; set;}
        public string brandId_Title { get; set;}
        public string unitId_Title { get; set;}
        public string type_Title { get; set;}
        public string stockType_Title { get; set;}
        public string status_Title { get; set;}
        public string isCriticalStock_Title { get; set;}
        public double? currentSellingPoint { get; set;}
        public string logo { get; set;}
        public double? currentBuyingPrice { get; set;}
        public Guid? currentBuyingCurrencyId { get; set;}
        public string currentBuyingCurrencyId_Title { get; set;}
        public double? currentSellingPrice { get; set;}
        public Guid? currentSellingCurrencyId { get; set;}
        public string currentSellingCurrencyId_Title { get; set;}
        public string currentSellingCurrencyCode { get; set;}
    }
}
