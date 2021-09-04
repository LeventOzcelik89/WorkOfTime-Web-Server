using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPA_Ledger : InfolineTable
    {
        public double totalAmount { get; set;}
        public decimal inputAmount { get; set;}
        public decimal outputAmount { get; set;}
        public double? balance { get; set;}
        public Guid? accountId { get; set;}
        public Guid? accountRealtedId { get; set;}
        public short? direction { get; set;}
        public double? amount { get; set;}
        public double? tax { get; set;}
        public double? rateExchange { get; set;}
        public Guid? currencyId { get; set;}
        public DateTime? date { get; set;}
        public short? type { get; set;}
        public Guid? transactionId { get; set;}
        public string description { get; set;}
        public bool? isBalanceFixing { get; set;}
        public double? crossRate { get; set;}
        public Guid? advanceId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string accountId_Title { get; set;}
        public string accountCompanyId_Title { get; set;}
        public string accountRealtedId_Title { get; set;}
        public string accountRelatedCompanyId_Title { get; set;}
        public string direction_Title { get; set;}
        public string currencyId_Title { get; set;}
        public string type_Title { get; set;}
        public string transactionId_Title { get; set;}
        public string Currency_Code { get; set;}
        public string Currency_Symbol { get; set;}
    }
}
