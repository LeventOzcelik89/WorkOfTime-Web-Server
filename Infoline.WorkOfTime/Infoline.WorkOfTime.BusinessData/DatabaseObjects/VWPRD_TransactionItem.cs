using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_TransactionItem : InfolineTable
    {
        public Guid? transactionId { get; set;}
        public Guid? productId { get; set;}
        public double? quantity { get; set;}
        public double? unitPrice { get; set;}
        public string serialCodes { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string transactionId_Title { get; set;}
        public string productId_Title { get; set;}
        public short? transactionType { get; set;}
        public string unitId_Title { get; set;}
    }
}
