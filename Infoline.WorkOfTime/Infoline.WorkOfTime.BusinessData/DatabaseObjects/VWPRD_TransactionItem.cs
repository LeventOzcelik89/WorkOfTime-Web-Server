using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_TransactionItem : InfolineTable
    {
        public Guid? transactionId { get; set;}
        public Guid? productId { get; set;}
        public double? unitPrice { get; set;}
        public string serialCodes { get; set;}
        public Guid? unitId { get; set;}
        public double? quantity { get; set;}
        public Guid? defaultUnitId { get; set;}
        public double? defaultQuantity { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string transactionId_Title { get; set;}
        public string productId_Title { get; set;}
        public short? transactionType { get; set;}
        public string transactionTypeTitle { get; set;}
        public string unitId_Title { get; set;}
        public string defaultUnitId_Title { get; set;}
        public string inputId_Title { get; set;}
        public string outputId_Title { get; set;}
        public string inputCompanyId_Title { get; set;}
        public string outputCompanyId_Title { get; set;}
    }
}
