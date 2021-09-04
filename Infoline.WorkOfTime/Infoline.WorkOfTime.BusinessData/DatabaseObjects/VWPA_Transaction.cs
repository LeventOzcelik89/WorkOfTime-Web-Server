using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPA_Transaction : InfolineTable
    {
        public decimal inputAmount { get; set;}
        public decimal outputAmount { get; set;}
        public int hasReject { get; set;}
        public int balance { get; set;}
        public string searchField { get; set;}
        public string Project_Title { get; set;}
        public string ProjectId { get; set;}
        public Guid? accountById { get; set;}
        public string accountData_Title { get; set;}
        public Guid? invoiceId { get; set;}
        public Guid? accountId { get; set;}
        public short? direction { get; set;}
        public short? type { get; set;}
        public double? amount { get; set;}
        public Guid? currencyId { get; set;}
        public DateTime? date { get; set;}
        public string description { get; set;}
        public DateTime? progressDate { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public short? status { get; set;}
        public double? tax { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string invoiceId_Title { get; set;}
        public string accountId_Title { get; set;}
        public string accountDataId_Title { get; set;}
        public string dataId_Title { get; set;}
        public string direction_Title { get; set;}
        public string status_Title { get; set;}
        public string type_Title { get; set;}
        public string currencyId_Title { get; set;}
        public string Currency_Code { get; set;}
        public string statusInfoMessage { get; set;}
        public string accountDataTable { get; set;}
        public Guid? accountDataId { get; set;}
        public decimal? debt { get; set;}
        public string Currency_Symbol { get; set;}
        public string confirmationUserIds { get; set;}
        public string confirmationUser_Title { get; set;}
        public short? confirmationStatus { get; set;}
        public string confirmUserIds { get; set;}
        public string confirmUser_Titles { get; set;}
        public string rejectedUser_Titles { get; set;}
        public string rejectedUserIds { get; set;}
    }
}
